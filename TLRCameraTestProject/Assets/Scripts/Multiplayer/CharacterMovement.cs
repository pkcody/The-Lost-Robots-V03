using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CharacterMovement : MonoBehaviour
{
    public Text displayInventory;
    public DisplayingInventory myInv;
    private PlayerControls playerInput;
    public GameObject body;
    public bool GameStarted;

    
    public float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    //private float gravityValue = -9.81f;

    private Rigidbody _rb;
    private CapsuleCollider _col;
    private bool doJump = false;
    

    public float distanceToGround = 0.1f;
    public LayerMask groundLayer;

    public Vector3 lastLook;
    public Vector2 movementInput;

    //Assign Robot info UI
    public GameObject robotInfo;

    //Inventory on/off
    public GameObject inventory;

    //Whats In Range
    public bool inRangeMonster;
    public bool inRangeResource;
    public bool inRangeCrafting;
    public bool offerItem;
    public bool inRangeHold;

    //Items in range
    [Header("Items in range")]
    public GameObject monster_obj;
    public GameObject monsterMain_obj;
    public GameObject resource_obj;
    public GameObject crafting_obj;
    public GameObject item_obj;

    //Sliders - health
    [Header("Sliders")]
    public Slider resourceSlider;

    //Crafting
    [Header("Crafting")]
    public GameObject craftBTImage;
    public GameObject craftTablePanel;
    public GameObject craftTableText;

    public List<ItemObject> AttemptedRecipe = new List<ItemObject>();
    private List<int> itemRow1Save = new List<int>();
    private List<int> itemRow2Save = new List<int>();

    [Header("Recipe Stuff")]
    // Recipe stuff
    public List<Image> CraftingSlots = new List<Image>();

    public GameObject craftingOptionsIndex;
    public List<TextMeshProUGUI> craftingTypeTexts;
    public int currentRecipeIndex;

    //Boarders
    [Header("Boarders")]
    public GameObject HitBoarder;
    public GameObject MonsterAttackBoarder;

    //health
    [Header("Health")]
    public List<Image> playerHealth = new List<Image>();
    public int playerHealthInt = 0;
    public int playerHealthMaxInt;
    public Sprite FullHealth;
    public Sprite DmgdHealth;

    // camera stuff
    [Header("Camera Stuff")]
    public CinemachineTargetGroup cinemachineTargetGroup;

    public float cameraOffsetX = 20f;
    public float cameraOffsetZ = 30f;
    public bool minX;
    public bool maxX;
    public bool minZ;
    public bool maxZ;

    //Holding
    [Header("Holding")]
    public CharHoldItem _charHoldItem;
    public Transform itemToHold;

    //Holding
    [Header("Animation")]
    public Animator animator;

    //Particles
    [Header("Particles")]
    public GameObject walkParticle;

    private void Awake()
    {
        playerInput = new PlayerControls();
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        myInv = GetComponent<DisplayingInventory>();
        _charHoldItem = GetComponent<CharHoldItem>();

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        //inventoryObj.Container.Clear();
        //invHolderText.gameObject.SetActive(false);
        StartCoroutine(CheckBoarderGlow(5f));
    }

    public void BeginGame()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            // Assigning Robot Info
            int playerIndex = System.Array.IndexOf(PlayerSpawning.instance.players, gameObject) + 1;
            robotInfo = GameObject.Find("RobotInfo_" + playerIndex);

            // Assign Inventory: For turning off and on inventory
            inventory = robotInfo.transform.GetChild(0).gameObject;
            //inventory.SetActive(false);

            foreach (Image image in robotInfo.transform.GetChild(2).GetComponentsInChildren<Image>())
            {
                playerHealth.Add(image);
                playerHealthInt++;
            }
            playerHealthInt--;
            playerHealthMaxInt = playerHealthInt;


            // find boarders
            foreach (var item in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (item.name.Contains("HitBoarder"))
                {
                    HitBoarder = item;
                    HitBoarder.SetActive(false);
                }
                else if (item.name.Contains("MonsterAttack"))
                {
                    MonsterAttackBoarder = item;
                }

            }

            GetComponentInChildren<FaceCamera>().SelectCamera();
            
        }
        
    }

    //player movement
    private void OnEnable()
    {
        playerInput.Enable();
        //BeginGame();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    public void OnShowInventory(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            print(inventory);
            inventory.SetActive(!inventory.activeSelf);
        }
    }

    public void OnTakeAction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (inRangeCrafting)
            {
                foreach (var item in crafting_obj.transform.GetComponentsInChildren<RectTransform>(true))
                {
                    if (item.name.Contains("CraftTableText"))
                    {
                        craftTableText = item.gameObject;
                    }
                    else if (item.name.Contains("BTImage"))
                    {
                        craftBTImage = item.gameObject;
                    }
                    else if (item.name.Contains("CraftMeOptions"))
                    {
                        craftingOptionsIndex = item.gameObject;
                    }
                }
                foreach (var item in crafting_obj.transform.GetComponentsInChildren<Transform>(true))
                {
                    if (item.name.Contains("CraftingPanel"))
                    {
                        craftTablePanel = item.gameObject;
                    }
                    
                }

                craftBTImage.SetActive(false);
                craftTablePanel.SetActive(true);
                craftTableText.SetActive(true);

                CraftingSlots.Clear();

                // add images from recipe
                foreach (var craftSlot in craftTableText.transform.GetChild(0).GetChild(2).GetComponentsInChildren<Image>())
                {
                    CraftingSlots.Add(craftSlot);
                }

                int index = 0;
                craftingTypeTexts.Clear();
                foreach (var item in craftingOptionsIndex.GetComponentsInChildren<TextMeshProUGUI>(true))
                {
                    if (craftingTypeTexts.Count < RecipeMaker.instance.recipes.Count)
                    {
                        craftingTypeTexts.Add(item);
                        craftingTypeTexts[index].text = RecipeMaker.instance.recipes[index].Name;


                        
                        index++;
                    }
                    item.gameObject.SetActive(false);
                }
                craftingTypeTexts[currentRecipeIndex].gameObject.SetActive(true);
                UpdateCraftingSlots();
            }
            if (inRangeMonster)
            {
                if(monster_obj != null)
                {
                    monster_obj.GetComponent<EnemyMove>().TakeDamage();
                    animator.Play("AttackRobot", 0, 0f);
                }
            }
            if (inRangeResource)
            {
                try
                {
                    resourceSlider = resource_obj.transform.GetComponentInChildren<Slider>();
                    resourceSlider.value -= 1;
                    if (resourceSlider.value == 0)
                    {
                        print("broke resource");
                        resource_obj.GetComponent<ResourceSelection>().GetResourceSource();
                        resource_obj.transform.GetChild(1).gameObject.SetActive(false);
                        resource_obj.transform.GetChild(2).gameObject.SetActive(false);
                        resource_obj.GetComponent<SphereCollider>().enabled = false;
                    }
                }
                catch
                {
                    print("no res");
                }
                
            }
            if (offerItem)
            {
                print("pick up item");
                animator.SetTrigger("pickUp");

            }
            if (inRangeHold)
            {
                print("hold item");
                _charHoldItem.HoldPlease(itemToHold);
            }
        }
    }

    
    public void UpdateCraftingSlots()
    {
        CraftingSlots[0].sprite = RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem1.UIimage;
        CraftingSlots[1].sprite = RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem2.UIimage;
        CraftingSlots[2].sprite = RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem3.UIimage;
        CraftingSlots[3].sprite = RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem4.UIimage;
        CraftingSlots[4].sprite = RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem5.UIimage;
    }

    // 3 Crafting buttons: 2 arrows to see craftables, 1 go to auto fill items and craft (will have delay and then show up in players inventory)
    public void OnCraftingNext(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && inRangeCrafting)
        {
            print("Craft next arrow");
            craftingTypeTexts[currentRecipeIndex].gameObject.SetActive(false);
            currentRecipeIndex = (currentRecipeIndex + 1) % craftingTypeTexts.Count;
            
            craftingTypeTexts[currentRecipeIndex].gameObject.SetActive(true);
            UpdateCraftingSlots();

        }
    }
    public void OnCraftingPrevious(InputAction.CallbackContext ctx)
    {
        
        if (ctx.performed && inRangeCrafting)
        {
            print("Craft prev arrow");
            craftingTypeTexts[currentRecipeIndex].gameObject.SetActive(false);
            if (currentRecipeIndex == 0)
            {
                currentRecipeIndex = craftingTypeTexts.Count - 1;
            }
            else
            {
                currentRecipeIndex -= 1;
            }
            
            craftingTypeTexts[currentRecipeIndex].gameObject.SetActive(true);
            UpdateCraftingSlots();

        }
    }

    public void OnCraftItem(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && inRangeCrafting)
        {
            AttemptedRecipe.Clear();
            itemRow1Save.Clear();
            itemRow2Save.Clear();

            if (RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem1.type != ItemType.Default)
            {
                AttemptedRecipe.Add(RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem1);
            }
            if (RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem2.type != ItemType.Default)
            {
                AttemptedRecipe.Add(RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem2);
            }
            if (RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem3.type != ItemType.Default)
            {
                AttemptedRecipe.Add(RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem3);
            }
            if (RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem4.type != ItemType.Default)
            {
                AttemptedRecipe.Add(RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem4);
            }

            for (int i = 0; i < 3; i++)
            {
                for (int a = 0; a < AttemptedRecipe.Count; a++)
                {
                    if (myInv.ItemsRow1UI[i].GetComponent<Image>().sprite == AttemptedRecipe[a].UIimage)
                    {
                        itemRow1Save.Add(i);
                        AttemptedRecipe.RemoveAt(a);
                        if (AttemptedRecipe.Count == 0)
                        {
                            GivePlayerCraftedItem();
                            return;
                        }
                        break;
                    }
                }
                
            }
            for (int i = 0; i < 3; i++)
            {
                for (int a = 0; a < AttemptedRecipe.Count; a++)
                {
                    if (myInv.ItemsRow2UI[i].GetComponent<Image>().sprite == AttemptedRecipe[a].UIimage)
                    {
                        itemRow2Save.Add(i);
                        AttemptedRecipe.RemoveAt(a);
                        if (AttemptedRecipe.Count == 0)
                        {
                            GivePlayerCraftedItem();
                            return;
                        }
                        break;
                    }
                }
            }

            offerItem = false;

        }
        if (ctx.performed && !inRangeCrafting)
        {

            myInv.HealReturn();

        }
    }

    public void GivePlayerCraftedItem()
    {
        foreach (int i in itemRow1Save)
        {
            myInv.ItemsRow1UI[i].GetComponent<Image>().sprite = RecipeMaker.instance.Empty.UIimage;
        }
        foreach (int i in itemRow2Save)
        {
            myInv.ItemsRow2UI[i].GetComponent<Image>().sprite = RecipeMaker.instance.Empty.UIimage;
        }

        if (RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem1.type != ItemType.Default)
        {
            myInv.inventoryObj.RemoveItem(RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem1);
        }
        if (RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem2.type != ItemType.Default)
        {
            myInv.inventoryObj.RemoveItem(RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem2);
        }
        if (RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem3.type != ItemType.Default)
        {
            myInv.inventoryObj.RemoveItem(RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem3);
        }
        if (RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem4.type != ItemType.Default)
        {
            myInv.inventoryObj.RemoveItem(RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem4);
        }

        myInv.inventoryObj.AddItem(RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem5, 1);
        GetComponent<RobotMessaging>().RobotSpeakResource(RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem5);

        for (int i = 0; i < 3; i++)
        {
            if (myInv.ItemsRow1UI[i].GetComponent<Image>().sprite.name.Contains("Red"))
            {
                myInv.ItemsRow1UI[i].GetComponent<Image>().sprite = RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem5.UIimage;
                return;
            }

        }
        for (int i = 0; i < 3; i++)
        {
            if (myInv.ItemsRow2UI[i].GetComponent<Image>().sprite.name.Contains("Red"))
            {
                myInv.ItemsRow2UI[i].GetComponent<Image>().sprite = RecipeMaker.instance.recipes[currentRecipeIndex].recipeItem5.UIimage;
                return;
            }

        }

        

    }

    public void OnScrollInvRight(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && inventory.activeSelf)
        {
            myInv.InventoryRight();
        }
    }
    public void OnScrollInvLeft(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && inventory.activeSelf)
        {
            myInv.InventoryLeft();
        }
    }
    public void OnDropItem(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            myInv.DropCurrentItem();
        }
        
    }

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && IsGrounded())
        {
            doJump = true;
        }
    }
    public void OnResetPlayers(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            foreach (var cm in FindObjectsOfType<CharacterMovement>())
            {
                cm.transform.position = new Vector3(transform.position.x, 30f, transform.position.z);
                cm.inRangeMonster = false;
                cm.inRangeResource = false;
                cm.inRangeCrafting = false;
                cm.offerItem = false;
                cm.inRangeHold = false;
            }
            
        }
    }

    //check if player on ground
    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom, distanceToGround, groundLayer, QueryTriggerInteraction.Ignore);
        return grounded;
    }

    public void PlayerTakeHeal()
    {
        if (playerHealthInt < playerHealthMaxInt)
        {
            print("taking heal");

            playerHealthInt++;
            playerHealth[playerHealthInt].sprite = FullHealth;

        }
    }
    public void PlayerTakeDamage()
    {
        if(playerHealthInt > 0)
        {
            //print("taking dmg");
            playerHealth[playerHealthInt].sprite = DmgdHealth;
            animator.SetTrigger("takeDmg");

            playerHealthInt--;

        }
        
    }
    
    public void OnTriggerExit(Collider collision)
    {
        if (collision.GetComponent<Collider>().tag == "TowerCraftingEncounter")
        {
            inRangeCrafting = false;
            if(craftBTImage != null)
                craftBTImage.SetActive(true);
            if (craftTablePanel != null)
                craftTablePanel.SetActive(false);
            if (craftTableText != null)
                craftTableText.SetActive(false);
        }
        if (collision.GetComponent<Collider>().tag == "MonsterEncounter")
        {
            inRangeMonster = false;
            //boarder
            MonsterAttackBoarder.SetActive(false);
        }
        if (collision.GetComponent<Collider>().tag == "ResourceEncounter")
        {
            inRangeResource = false;
        }
        if (collision.GetComponent<Collider>().tag == "ResourceItem")
        {
            offerItem = false;
        }
        if (collision.GetComponent<Collider>().tag == "BoarderBoundry")
        {
            HitBoarder.SetActive(false);
        }
        if (collision.GetComponent<Collider>().tag == "HoldItem")
        {
            inRangeHold = false;
        }

    }
    
    public void OnTriggerEnter(Collider collision)
    {
        print(collision.name);
        if (collision.GetComponent<Collider>().tag == "TowerCraftingEncounter")
        {
            crafting_obj = collision.gameObject;
            inRangeCrafting = true;
        }
        if (collision.GetComponent<Collider>().tag == "MonsterEncounter")
        {
            //for dmg
            print("ahh");
            //monsterMain_obj = collision.gameObject;
            monster_obj = collision.transform.parent.gameObject;
            inRangeMonster = true;
            if (monster_obj.GetComponentInChildren<Animator>().GetBool("attack"))
            {
                PlayerTakeDamage();

            }
            //boarder
            MonsterAttackBoarder.SetActive(true);
        }
        if (collision.GetComponent<Collider>().tag == "ResourceEncounter")
        {
            resource_obj = collision.gameObject;
            inRangeResource = true;
        }
        if (collision.GetComponent<Collider>().tag == "ResourceItem")
        {
            item_obj = collision.gameObject;
            offerItem = true;
            
        }
        if (collision.GetComponent<Collider>().tag == "BoarderBoundry")
        {
            HitBoarder.SetActive(true);
        }
        if (collision.GetComponent<Collider>().tag == "Mothership")
        {
            //FindObjectOfType<Mothership>().CheckHasParts();
        }
        if (collision.GetComponent<Collider>().tag == "HoldItem")
        {
            inRangeHold = true;
            itemToHold = collision.gameObject.transform;
        }

    }


    IEnumerator CheckBoarderGlow(float startDelay = 0f)
    {
        yield return new WaitForSeconds(startDelay);

        if(!minX || !minZ || !maxX || !maxZ)
        {
            if(!HitBoarder.activeSelf)
            {
                HitBoarder.SetActive(true);
            }
        }
        else
        {
            HitBoarder.SetActive(false);
        }

        yield return new WaitForSeconds(.5f);

        StartCoroutine(CheckBoarderGlow());
    }

    //update
    private void Update()
    {
        
        minX = transform.position.x > cinemachineTargetGroup.transform.position.x - cameraOffsetX;
        maxX = transform.position.x < cinemachineTargetGroup.transform.position.x + cameraOffsetX;
        minZ = transform.position.z > cinemachineTargetGroup.transform.position.z - cameraOffsetZ;
        maxZ = transform.position.z < cinemachineTargetGroup.transform.position.z + cameraOffsetZ;
        

        if ((movementInput.x < 0 && minX || movementInput.x > 0 && maxX) && (movementInput.y < 0 && minZ || movementInput.y > 0 && maxZ))
        {
            transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * playerSpeed * Time.deltaTime);
            _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);
            
            if (movementInput.x != 0f || movementInput.y != 0f)
            {
                lastLook = new Vector3(movementInput.x, 0, movementInput.y);

                animator.SetBool("isRun", true);
                walkParticle.SetActive(true);


            }

            body.transform.forward = lastLook;

        }
        
        else
        {
            walkParticle.SetActive(false);

            animator.SetBool("isRun", false);

        }

        if (transform.position.y < -50f)
        {
            transform.position = new Vector3(transform.position.x, 30f, transform.position.z);
        }
    }


    private void FixedUpdate()
    {
        if (doJump)
        {
            _rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            doJump = false;
        }

    }
}
