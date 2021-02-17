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
  class SpaceGame
  {
    public bool Running { get; set; }
    public bool inShop { get; set; }
    public int Cheese { get; private set; }
    public List<Upgrade> Shop { get; private set; }
    public List<Upgrade> ClickUpgrades { get; private set; }
    public List<Upgrade> AutoUpgrades { get; private set; }
    public Dictionary<string, int> Stats { get; set; }

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
        var input = GetPlayerInput();
        switch (input.Key.ToString().ToLower())
        {
          case "spacebar":
            Mine();
            break;
          case "s":
            GoToShop();
            break;
        }
      }
    }
    public ConsoleKeyInfo GetPlayerInput()
    {
      DrawGameScreen();
      return Console.ReadKey();
    }

    public void Mine()
    {
      ClickUpgrades.ForEach(miner =>
      {
        Cheese += miner.Quantity * miner.GenValue;
      });
      Console.WriteLine($"Cheese:{Cheese}");
    }

    public void AutoMine(object o)
    {
      AutoUpgrades.ForEach(miner =>
   {
     Cheese += (miner.Quantity * miner.GenValue) * 5;
   });
      if (inShop == false)
      {
        DrawGameScreen();
      }
    }
    public void StartTimer()
    {
      Timer timer = new Timer(AutoMine, null, 0, 5000);
    }

    public void GoToShop()
    {
      inShop = true;
      Console.Clear();
      Console.WriteLine("Welcome to the eht shopp'd, what would you like to buy?");
      string message = "";
      for (int i = 0; i < Shop.Count; i++)
      {
        Upgrade item = Shop[i];
        message += $"{i + 1}. {item.Name}: ${item.Price}, Generates: {item.GenValue} \n";
      }
      Console.WriteLine(message);
      string choice = Console.ReadLine();
      if (int.TryParse(choice, out int selection) && selection > 0 && selection <= Shop.Count)
      {
        BuyUpgrade(selection - 1);
      }
    }

    public void BuyUpgrade(int shopIndex)
    {
      Upgrade item = Shop[shopIndex];
      if (Cheese >= item.Price)
      {
        Cheese -= item.Price;
        item.Price += item.Price;
        if (item.Type == "click")
        {
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
      Console.WriteLine("press any key to continue");
      Console.ReadKey();
      inShop = false;
    }






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
