using Microsoft.Data.SqlClient;
using smtoffice.Infrastructure.Interfaces;
using smtOffice.Application.Interfaces;
using smtOffice.Domain.Entity.dropdown;

namespace smtoffice.Infrastructure.Common
{
    public class DataSeeder(ISqlConnectionFactory sqlConnectionFactory, IPasswordHasher passwordHasher)
    {
        private readonly ISqlConnectionFactory sqlConnectionFactory = sqlConnectionFactory;

        public void Seed()
        {
            using var connection = sqlConnectionFactory.CreateConnection();
            if (connection != null)
            {
                var roles = new List<Position>();

                // Check if any roles exist in the database
                var queryCheckPositions = "SELECT COUNT(*) FROM Position";
                var queryCheckAdmnins = "SELECT COUNT(*) FROM Employees WHERE Position = 'admin'";
                var queryCheckAbsenceReason = "SELECT COUNT(*) FROM AbsenceReason";
                var queryCheckSubdivision = "SELECT COUNT(*) FROM Subdivision";

                var commandPositions = new SqlCommand(queryCheckPositions, connection);
                var commandAdmins = new SqlCommand(queryCheckAdmnins, connection);
                var commandAbsenceReason = new SqlCommand(queryCheckAbsenceReason, connection);
                var commandSubdivision = new SqlCommand(queryCheckSubdivision, connection);

                connection.Open();

                var positionCount = (int)commandPositions.ExecuteScalar();
                var adminCount = (int)commandAdmins.ExecuteScalar();
                var AbsenceReasonCount = (int)commandAbsenceReason.ExecuteScalar();
                var SubdivisionCount = (int)commandSubdivision.ExecuteScalar();

                if (positionCount == 0)
                {
                    var predefinedPositions = new List<Position>
                    {
                        new Position { Name = "employee" },
                        new Position { Name = "hrmanager" },
                        new Position { Name = "projectmanager" },
                        new Position { Name = "admin" }
                    };

                    foreach (var position in predefinedPositions)
                    {
                        var insertQuery = "INSERT INTO Position (Name) VALUES (@Name)";
                        using (var commandInsert = new SqlCommand(insertQuery, connection))
                        {
                            commandInsert.Parameters.AddWithValue("@Name", position.Name);
                            commandInsert.ExecuteNonQuery();
                        }
                    }
                }
                // If no admin exist, seed the database with predefined admin
                if (adminCount == 0)
                {
                    var insertEmployeeQuery = @"
                                INSERT INTO Employees (Username, PasswordHash, FullName, Subdivision, Position, Status, PeoplePartnerID, OutOfOfficeBalance) 
                                VALUES (@Username, @PasswordHash, @FullName, @Subdivision, @Position, @Status, NULL, @OutOfOfficeBalance);
    
                                SELECT SCOPE_IDENTITY();";

                    var commandAdminInsert = new SqlCommand(insertEmployeeQuery, connection);

                    var hashedPassword = passwordHasher.HashPassword("admin");

                    commandAdminInsert.Parameters.AddWithValue("@Username", "admin");
                    commandAdminInsert.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                    commandAdminInsert.Parameters.AddWithValue("@FullName", "Adam Administrowicz");
                    commandAdminInsert.Parameters.AddWithValue("@Subdivision", "IT");
                    commandAdminInsert.Parameters.AddWithValue("@Position", "admin");
                    commandAdminInsert.Parameters.AddWithValue("@Status", "Active");
                    commandAdminInsert.Parameters.AddWithValue("@OutOfOfficeBalance", 20);

                    var result = commandAdminInsert.ExecuteScalar();
                    var employeeID = Convert.ToInt32(result);

                    // Now update the same employee with its own ID as PeoplePartnerID
                    var updateEmployeeQuery = @"
                                UPDATE Employees 
                                SET PeoplePartnerID = @PeoplePartnerID 
                                WHERE ID = @EmployeeID";

                    var commandAdminUpdate = new SqlCommand(updateEmployeeQuery, connection);

                    commandAdminUpdate.Parameters.AddWithValue("@PeoplePartnerID", employeeID);
                    commandAdminUpdate.Parameters.AddWithValue("@EmployeeID", employeeID);

                    commandAdminUpdate.ExecuteNonQuery();
                }
                if (AbsenceReasonCount == 0)
                {
                    var predefinedReasons = new List<AbsenceReason>
                    {
                        new AbsenceReason { Name = "Unpaid leave" },
                        new AbsenceReason { Name = "Training leave" },
                        new AbsenceReason { Name = "Vacation leave" },
                        new AbsenceReason { Name = "on-demand leave" }
                    };

                    foreach (var reason in predefinedReasons)
                    {
                        var insertQuery = "INSERT INTO AbsenceReason (Name) VALUES (@Name)";
                        using (var commandInsert = new SqlCommand(insertQuery, connection))
                        {
                            commandInsert.Parameters.AddWithValue("@Name", reason.Name);
                            commandInsert.ExecuteNonQuery();
                        }
                    }
                }
                if (SubdivisionCount == 0)
                {
                    var predefinedSubdivisions = new List<Subdivision>
                    {
                        new Subdivision { Name = "Information Technology" },
                        new Subdivision { Name = "Human Resource" },
                        new Subdivision { Name = "Finance" },
                        new Subdivision { Name = "Marketing" }
                    };

                    foreach (var subdivision in predefinedSubdivisions)
                    {
                        var insertQuery = "INSERT INTO Subdivision (Name) VALUES (@Name)";
                        using (var commandInsert = new SqlCommand(insertQuery, connection))
                        {
                            commandInsert.Parameters.AddWithValue("@Name", subdivision.Name);
                            commandInsert.ExecuteNonQuery();
                        }
                    }
                }
            }

        }
    }
}
