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
            string category = "";
            //Uses EPPlus for .NetCore - installed via Package Manager
            var package = new ExcelPackage(new FileInfo("Inventory_List.xlsx"));
            ExcelWorksheet workSheet = package.Workbook.Worksheets["Sheet1"];
            
            for (int i = 2; i <= workSheet.Dimension.End.Row; i++)
            {
                

                ExcelRange cell = workSheet.Cells[i, 1];
                if(cell.Style.Fill.BackgroundColor.Rgb != Color.White.ToArgb().ToString())
                {    
                    object cellValue = workSheet.Cells[i, 1].Value;
                    category = cellValue.ToString();
                    //Grabs the Category. Categories are currently written with Orange Backgrounds
                    //every few lines. Saves each one temporarily to apply it to successive items
                }
                else
                {
                    //Set values based on values present in excel
                    //If/Else used to see if cell is blank in excel
                    Equipment temp = new Equipment();
                    temp.Category = category; //Grabbed From Above
                    temp.Status = "Active"; //Default Value
                    if(workSheet.Cells[i, 1].Value == null)
                    { temp.Name = "NO NAME FOUND"; }
                    else { temp.Name = workSheet.Cells[i, 1].Value.ToString(); }
                    if (workSheet.Cells[i, 2].Value == null)
                    { temp.Quantity = -99; }
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

                    inventory.Add(temp);
                }
                
                
            }
            //{
            //new Equipment{ Name="X32",Quantity=1,Description="Behringer X32 Digital Mixer", Location="Upstairs At Church", Link="https://www.sweetwater.com/store/detail/X32", Category = "Mixer"},
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
