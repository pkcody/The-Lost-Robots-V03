using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{
    public string Name;

    public ItemObject recipeItem1;
    public ItemObject recipeItem2;
    public ItemObject recipeItem3;
    public ItemObject recipeItem4;
    public ItemObject recipeItem5;

    
    public Recipe(string name, ItemObject i1, ItemObject i2, ItemObject i3, ItemObject i4, ItemObject i5)
    {
        Name = name;
        recipeItem1 = i1;
        recipeItem2 = i2;
        recipeItem3 = i3;
        recipeItem4 = i4;
        recipeItem5 = i5;
    }

}

public class RecipeMaker : MonoBehaviour
{
    public static RecipeMaker instance;

    // Resources - Res
    public ItemObject WoodRes;
    public ItemObject MetalRes;
    public ItemObject CopperRes;
    public ItemObject GoldRes;
    public ItemObject SteelRes;
    public ItemObject RubberRes;
    public ItemObject PlasticRes;
    public ItemObject AluminumRes;
    public ItemObject PolymerRes;
    public ItemObject Empty;

    //building this -------------------
    // Towers
    public ItemObject TowerBuild2;
    public ItemObject TowerBuild3;
    
    //Advance - Adv
    public ItemObject FanAdv;
    public ItemObject BatteryAdv;

    //old
    public ItemObject BodyPartHeadRes;
    public ItemObject Tower2Res;
    public ItemObject WheelRes;

    // Tiny Tasks - TT
    //public ItemObject WiresTT;
    public ItemObject SatelliteTT;
    public ItemObject WiresWeakTT;
    public ItemObject WiresStrongTT;
    public ItemObject SolarPanelsTT;
    public ItemObject TiresTT;
    public ItemObject FourCellsTT;




    // Body Armor - BA

    //class ----------------
    //V01
    public Recipe Head;
    public Recipe Tower2;
    public Recipe Wheels;

    //V02
    // Resources - Res
    //public Recipe rWoodRes;
    //public Recipe rMetalRes;
    //public Recipe rCopperRes;
    //public Recipe rGoldRes;
    //public Recipe rBatteryRes;
    //public Recipe rSteelRes;
    //public Recipe rRubberRes;
    //public Recipe rPlasticRes;
    //public Recipe rAluminumRes;
    //public Recipe rPolymerRes;
    //public Recipe rEmpty;

    //building this -------------------
    // Towers
    public Recipe rTowerBuild2;
    public Recipe rTowerBuild3;

    //Advance - Adv
    public Recipe rFanAdv;
    public Recipe rBatteryAdv;

    //old
    public Recipe rBodyPartHeadRes;
    public Recipe rTower2Res;
    public Recipe rWheelRes;

    // Tiny Tasks - TT
    //public Recipe rWiresTT;
    public Recipe rSatelliteTT;
    public Recipe rWiresWeakTT;
    public Recipe rWiresStrongTT;
    public Recipe rSolarPanelsTT;
    public Recipe rTiresTT;
    public Recipe rFourCellsTT;


    //-------------

    public List<Recipe> recipes = new List<Recipe>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CreateRecipes();
    }
    public void CreateRecipes()
    {
        //V01
        //Head = new Recipe("Rare Head", WoodRes, MetalRes, Empty, Empty, BodyPartHeadRes);
        //Tower2 = new Recipe("Tower 2 old", WoodRes, WoodRes, WoodRes, Empty, Tower2Res);
        //Wheels = new Recipe("Wheels", WoodRes, MetalRes, MetalRes, MetalRes, WheelRes);

        //V02--------
        //Towers
        //rTowerBuild2 = new Recipe("Tower 2", SatelliteTT, WoodRes, BatteryAdv, WiresWeakTT, TowerBuild2);
        rTowerBuild2 = new Recipe("Tower 2", SatelliteTT, WoodRes, WiresStrongTT, WiresWeakTT, TowerBuild2); 
        rTowerBuild3 = new Recipe("Tower 3", SatelliteTT, WoodRes, SolarPanelsTT, TiresTT, TowerBuild3);
        //Advance Tasks
        rFanAdv = new Recipe("Fan", SteelRes, AluminumRes, PolymerRes, Empty, FanAdv);
        //rBatteryAdv = new Recipe("Battery", WiresStrongTT, FourCellsTT, Empty, Empty, BatteryAdv);
        //Tiny Tasks
        //rWiresTT = new Recipe("Wires", CopperRes, Empty, Empty, Empty, WiresTT);
        rSatelliteTT = new Recipe("Satellite", PlasticRes, AluminumRes, SteelRes, Empty, SatelliteTT);
        rWiresWeakTT = new Recipe("Wires: Weak", CopperRes, Empty, Empty, Empty, WiresWeakTT);
        rWiresStrongTT = new Recipe("Wires: Strong", GoldRes, Empty, Empty, Empty, WiresStrongTT);
        rSolarPanelsTT = new Recipe("Solar Panels", AluminumRes, WiresWeakTT, GoldRes, PlasticRes, SolarPanelsTT);
        rTiresTT = new Recipe("Tires", RubberRes, RubberRes, Empty, Empty, TiresTT);
        //rFourCellsTT = new Recipe("Four Cells", SingleCellRes, SingleCellRes, SingleCellRes, SingleCellRes, FourCellsTT);



        //V01
        //recipes.Add(Head);
        //recipes.Add(Tower2);
        //recipes.Add(Wheels);
        //V02
        recipes.Add(rTowerBuild2);
        recipes.Add(rTowerBuild3);
        recipes.Add(rFanAdv);
        //recipes.Add(rBatteryAdv);

        recipes.Add(rSatelliteTT);
        recipes.Add(rWiresWeakTT);
        recipes.Add(rWiresStrongTT);
        recipes.Add(rSolarPanelsTT);
        recipes.Add(rTiresTT);
    }
}