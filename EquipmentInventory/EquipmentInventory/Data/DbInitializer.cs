using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EquipmentInventory.Models;
using OfficeOpenXml;
using System.IO;
using System.Drawing;

namespace EquipmentInventory.Data
{
    public static class DbInitializer
    {
        public static void Initialize(InventoryContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Equipment.Any())
            {
                return;   // DB has been seeded
            }

            var inventory = new List<Equipment>();
            string category = "", tempStr = "";
            Equipment temp = new Equipment();
            //Uses EPPlus for .NetCore - installed via Package Manager
            var package = new ExcelPackage(new FileInfo("C:\\Users\\schma\\Desktop\\Equipment_Inventory\\EquipmentInventory\\EquipmentInventory\\Inventory_List.xlsx"));
            ExcelWorksheet workSheet = package.Workbook.Worksheets["Sheet1"];

            for (int i = 2; i <= workSheet.Dimension.End.Row; i++)
            {


                ExcelRange cell = workSheet.Cells[i, 1];
                if (workSheet.Cells[i, 2] == null)
                {
                    //object cellValue = workSheet.Cells[i, 1].Value;
                    category = workSheet.Cells[i, 1].Value.ToString();
                    //Grabs the Category. Categories are currently written with Orange Backgrounds
                    //every few lines. Saves each one temporarily to apply it to successive items
                    //This recognizes a cell as a category if the second cell in the column is blank
                }
                else
                {
                    //Set values based on values present in excel
                    //If/Else used to see if cell is blank in excel
                    //Category set during Add to List
                    temp.Status = "Active"; //Default Value
                    if (workSheet.Cells[i, 1].Value == null)
                    { temp.Name = "NO NAME FOUND"; }
                    else { temp.Name = workSheet.Cells[i, 1].Value.ToString(); }
                    if (workSheet.Cells[i, 2].Value == null)
                    { temp.Quantity = -99; }
                    else if (workSheet.Cells[i, 2].Value.ToString().Contains("?"))
                    {
                        tempStr = workSheet.Cells[i, 2].Value.ToString();
                        tempStr.Remove(tempStr.Length - 1);
                        try
                        {
                            temp.Quantity = Int32.Parse(tempStr);
                        }
                        catch
                        {
                            temp.Quantity = -1;
                        }
                    }
                    else { temp.Quantity = Int32.Parse(workSheet.Cells[i, 2].Value.ToString()); }
                    if (workSheet.Cells[i, 3].Value == null)
                    { temp.Description = "No Description"; }
                    else { temp.Description = workSheet.Cells[i, 3].Value.ToString(); }
                    if (workSheet.Cells[i, 4].Value == null)
                    { temp.Link = "No Link"; }
                    else { temp.Link = workSheet.Cells[i, 4].Value.ToString(); }
                    if (workSheet.Cells[i, 5].Value == null)
                    { temp.Location = "No Location"; }
                    else { temp.Location = workSheet.Cells[i, 5].Value.ToString(); }

                    inventory.Add(new Equipment
                    {
                        Category = category, //Grabbed from inside if statement that begins loop iteration.
                        Description = temp.Description,
                        Link = temp.Link,
                        Location = temp.Location,
                        Name = temp.Name,
                        Quantity = temp.Quantity,
                        Status = temp.Status
                    }
                    );
                }


            }
            //{
            //temp = new Equipment { Name = "X32", Quantity = workSheet.Dimension.End.Column, Description = workSheet.Cells[2, 1].Style.Fill.BackgroundColor.LookupColor(), Location = "COOL", Link = "https://www.sweetwater.com/store/detail/X32", Category = "Mixer" };
            //inventory.Add(temp);
            //new Equipment{ Name="Mini Board",Quantity=1,Description="PsudoDigital Board", Category = "Mixer" },
            //new Equipment{ Name="S16",Quantity=1,Description="Behringer S16 16 Channel Digital Snake", Location="On Stage", Link="https://www.sweetwater.com/store/detail/S16", Category = "Snakes"},
            //};
            foreach (Equipment s in inventory)
            {
                context.Equipment.Add(s);
            }
            context.SaveChanges();
        }
    }
}
