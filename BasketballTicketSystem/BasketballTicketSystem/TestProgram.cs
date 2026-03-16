using System;

class TestProgram {
    static void Main(string[] args) {
        Console.WriteLine("Starting Database Test");

        try {
            IEmployeeRepository employeeRepo = new EmployeeDBRepository();

            var employees = employeeRepo.FindAll();

            Console.WriteLine($"Success! Connected to the database and found {employees.Count} employees.");
            Console.WriteLine("Check bin/Debug folder for the application.log file to see the NLog output!");
        }
        catch (Exception ex) {
            Console.WriteLine("Something went wrong:");
            Console.WriteLine(ex.Message);
        }

        Console.ReadLine();
    }
}