using System;
using System.Collections.Generic;
using System.Threading;

namespace SpaceGame
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Welcome to SharpMiner");
      new SpaceGame();
    }
  }


  // object class used to build out our upgrades
  class Upgrade
  {
    public string Name { get; private set; }
    public string Type { get; private set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public int GenValue { get; private set; }

    public Upgrade(string name, string type, int price, int quantity, int genValue)
    {
      Name = name;
      Type = type;
      Price = price;
      Quantity = quantity;
      GenValue = genValue;
    }
  }

  //   Game class that wil hold the data of the game.
  class SpaceGame
  {
    public bool Running { get; set; }
    public bool inShop { get; set; }
    public int Cheese { get; private set; }
    public List<Upgrade> Shop { get; private set; }
    public List<Upgrade> ClickUpgrades { get; private set; }
    public List<Upgrade> AutoUpgrades { get; private set; }
    public Dictionary<string, int> Stats { get; set; }

    // The constructor is basically the new game setup, it creates an instance of all the Lists and Dictionaries needed, and creates the shop items, and gives the player 1 pick axe to start.
    public SpaceGame()
    {
      Running = true;
      Shop = new List<Upgrade>() { };
      ClickUpgrades = new List<Upgrade>() { new Upgrade("Pick Axe", "click", 0, 1, 1) };
      AutoUpgrades = new List<Upgrade>();
      Stats = new Dictionary<string, int>();
      Stats.Add("Pick Axe", 1);
      Shop.Add(new Upgrade("Pick Axe", "click", 25, 0, 1));
      Shop.Add(new Upgrade("Cheese Drill", "click", 50, 0, 5));
      Shop.Add(new Upgrade("Mousetronaut", "auto", 100, 0, 1));
      Shop.Add(new Upgrade("Cheese Refiner", "auto", 1000, 0, 10));
      PlayGame();
    }
    public void PlayGame()
    {
      StartTimer();
      while (Running)
      {
        //   Get player input captures the players key press, and converts it from a "ConsoleKeyinfo" into a string we can check.
        string input = GetPlayerInput().Key.ToString().ToLower();
        switch (input)
        {
          case "spacebar":
            Mine();
            break;
          case "s":
            GoToShop();
            break;
          case "escape":
            Running = false;
            break;
        }
      }
    }

    // Draws the screen (the moon), and then waits to read the user's key press.
    public ConsoleKeyInfo GetPlayerInput()
    {
      DrawGameScreen();
      return Console.ReadKey();
    }

    // iterates over click mining objects and adds their Quantity * generation Value to you Cheese.
    public void Mine()
    {
      ClickUpgrades.ForEach(miner =>
      {
        Cheese += miner.Quantity * miner.GenValue;
      });
      Console.WriteLine($"Cheese:{Cheese}");
    }

    // iterates over automine objects and adds their Quantity * generation Value to you Cheese. Timer callbacks need to take in an object as a parameter.
    public void AutoMine(object o)
    {
      AutoUpgrades.ForEach(miner =>
   {
     Cheese += (miner.Quantity * miner.GenValue) * 5;
   });
      //  keeps our auto interval from interupting our user while in another game screen
      if (inShop == false)
      {
        DrawGameScreen();
      }
    }
    // starts the interval to automatically add our cheese items
    public void StartTimer()
    {
      Timer timer = new Timer(AutoMine, null, 0, 5000);
    }


    // opens the shop Menu
    public void GoToShop()
    {
      inShop = true;
      Console.Clear();
      Console.WriteLine("Welcome to the eht shopp'd, what would you like to buy?");
      string message = "";
      //   iterates over the items available in the shop and displays them with a number corresponding to the purchase we want to make.
      for (int i = 0; i < Shop.Count; i++)
      {
        Upgrade item = Shop[i];
        message += $"{i + 1}. {item.Name}: ${item.Price}, Generates: {item.GenValue} \n";
      }
      Console.WriteLine(message);
      string choice = Console.ReadLine();
      //   Checks to see if choice of player is a valid selection.
      if (int.TryParse(choice, out int selection) && selection > 0 && selection <= Shop.Count)
      {
        BuyUpgrade(selection - 1);
      }
    }

    // Purchases an item from the shop menu
    public void BuyUpgrade(int shopIndex)
    {
      Upgrade item = Shop[shopIndex];
      if (Cheese >= item.Price)
      {
        Cheese -= item.Price;
        item.Price += item.Price;
        if (item.Type == "click")
        {
          // This checks to see if we already have purchased this upgrade, if we have not, we will add it to our upgrades, if we do it will increment the quantity of the upgrade we have.
          int index = ClickUpgrades.FindIndex(i => i.Name == item.Name);
          if (index == -1)
          {
            ClickUpgrades.Add(item);
            index = ClickUpgrades.Count - 1;
          }
          ClickUpgrades[index].Quantity++;
          Stats[item.Name] = ClickUpgrades[index].Quantity;
        }
        else
        {
          // This checks to see if we already have purchased this upgrade, if we have not, we will add it to our upgrades, if we do it will increment the quantity of the upgrade we have.
          int index = AutoUpgrades.FindIndex(i => i.Name == item.Name);
          if (index == -1)
          {
            AutoUpgrades.Add(item);
            index = AutoUpgrades.Count - 1;
          }
          AutoUpgrades[index].Quantity++;
          Stats[item.Name] = AutoUpgrades[index].Quantity;
        }
        Console.Beep();
        Console.WriteLine($"You Purchezed 1 {item.Name}... tank you for you buzinez");
      }
      else
      {
        Console.WriteLine($"you don't heav enough cheez to by {item.Name}... go away.");
      }
      //   Using a Readkey so the user has a moment to read the message before they continue.
      Console.WriteLine("press any key to continue");
      Console.ReadKey();
      inShop = false;
    }







    // draws the games "screen"  keeping the amount of "Console.WritLines()" down to help with the responsiveness of the progarm.
    public void DrawGameScreen()
    {
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.DarkYellow;
      string moon = $@"
                                                                                      
                              %%&&&&&&%                                         
                       %%%%%%%&%%%%&&&&&&&&&&&                                  
                   (###(((###%###%%&%&&%&%###(%&&%                              
                 ,***///(####((///(%%%##%%%&&%&&&%&%                            
                 .,*,*/(/#(/*//((/(///(///(%#&(((#%&&&                          
                  ..,,*/////**////////(///(((#(#((#%&&&&                        
                  ....,*/**(/**/*/#(**////(%#%%##(#%%&%&&                       
                    ..,.,,#/***,*/(##%(((%(##%%%%%%%%&&&&&                      
                     ...,,,,,*//((####(#&%%%&%###(%%&&&&&&&                     
                      ...,,..,/*/(((####%#%%%%%&#%%%&&@@&&&                     
                        .....,**////#((##%#%%%%&%&%&&&&&&&%                     
                         ....,,**////((((##%##&%%%%%&&&%%&%                     
                            .....,***//#((###%&%#%%%%&&%&%%                     
                              ......**((#######%#%%%%%%%&%                      
                                .....,*(#(#(##%((#%%%%%&%                       
                                  ...,**/##/(#%###%##%%%                        
                                      * * /#(((((% #%#                          
                                         ..*,@* /(%&                            
                                                                                
      ";
      Console.WriteLine(moon);
      string message = "";
      message += $@"
      Mine [Space],  Shop [tab], Quit[esc]
      Cheese: {Cheese}
      Stats:";
      foreach (var stat in Stats)
      {
        message += $"\n    {stat.Key} : {stat.Value}";
      }
      Console.ForegroundColor = ConsoleColor.DarkCyan;
      Console.WriteLine(message);
    }
  }


}
