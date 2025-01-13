using publisherData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumplingsEFCore
{
    public class StaffManager
    {
        private readonly PubContext context;

        public StaffManager(PubContext _context)
        {
            context = _context;
        }

        public void StartMenu()
        {
            bool exit = false;
            while (!exit)
            {

                string choice = InputHelper.GetUserInput<string>(
                    "Välkommen, här kan du hantera personal\n" +
                    "1: Printa ut Personal\n" +
                    "2: Lägg till Personal\n" +
                    "3: Uppdatera Personal\n" +
                    "4: Ta bort Personal\n" +
                    "5: Gå tillbaka till menyn\n" +
                    "6: Avsluta program"
                    );

                switch (choice)
                {
                    case "1":
                        PrintStaff();
                        break;
                    case "2":
                        AddStaff();
                        break;
                    case "3":
                        UpdateStaff();
                        break;
                    case "4":
                        RemoveStaff();
                        break;
                    case "5":
                        exit = true;
                        break;
                    case "6":
                        Program.CloseProgram();
                        break;
                    default:
                        Console.WriteLine("Ogiltig input försök igen");
                        break;

                }
            }
        }

        public void PrintStaff()
        {
            var staff = context.Staff
                .OrderBy(x => x.Name)
                .ToList();

            Console.WriteLine("Personal: ");
            foreach (var x in staff)
            {
                Console.WriteLine($"Namn: {x.Name}, ({x.Id}), {x.Role}, telefon: {x.Telephone}");
            }
        }
        public void AddStaff()
        {

            string name = InputHelper.GetUserInput<string>("Skriv in namn:");
            string telephone = InputHelper.GetUserInput<string>("Skriv in telefonnummer:");
            string role = InputHelper.GetUserInput<string>("Ange roll: ");

            var newStaff = new Staff
            {
                Name = name,
                Telephone = telephone,
                Role = role
            };

            context.Staff.Add(newStaff);
            context.SaveChanges();
            Console.WriteLine("Personal tillagd!");
        }

        public Staff SearchForStaff()
        {
            string updateStaff = InputHelper.GetUserInput<string>("Ange namn på den personal du vill hitta: ");
            var person = context.Staff.FirstOrDefault(p => p.Name.ToLower().Equals(updateStaff.ToLower()));

            if (person == null) {
                Console.WriteLine("Person hittades inte");
            }
            return person;
        }

        public void UpdateStaff()
        {
            var person = SearchForStaff();
            Console.WriteLine($"Uppdatera {person.Name}: ");
            person.Name = InputHelper.GetUserInput<string>("Namn:");
            person.Telephone = InputHelper.GetUserInput<string>("Telefon:");
            person.Role = InputHelper.GetUserInput<string>("Roll:");
            context.SaveChanges();

            Console.WriteLine($"Uppdatering sparad: {person.Name} (ID: {person.Id})");
        }

        public void RemoveStaff()
        {
            var person = SearchForStaff();

            context.Staff.Remove(person);
            context.SaveChanges();
            Console.WriteLine($"{person.Name} (ID: {person.Id}) har tagits bort.");
        }















    }
}
