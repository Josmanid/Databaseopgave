using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Databaseopgave
{
    class DBClient
    {
        // Database connection string
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HoteldB;Integrated Security=True;";

        private int GetMaxFacilityId(SqlConnection connection) {
            Console.WriteLine("Calling -> GetMaxFacilityId");

            string queryStringMaxFacilityId = "SELECT MAX(Facility_id) FROM Facilities";
            Console.WriteLine($"SQL applied: {queryStringMaxFacilityId}");

            SqlCommand command = new SqlCommand(queryStringMaxFacilityId, connection);
            //ExecuteReader(): Used for executing SQL queries that return rows of data, such as SELECT statements.
            SqlDataReader reader = command.ExecuteReader();

            int maxFacilityId = 0;
            if (reader.Read())
            {
                maxFacilityId = reader.GetInt32(0);
            }

            reader.Close();
            Console.WriteLine($"Max Facility ID: {maxFacilityId}");
            return maxFacilityId;
        }

        private int DeleteFacility(SqlConnection connection, int facilityId) {
            Console.WriteLine("Calling -> DeleteFacility");

            string deleteCommandString = $"DELETE FROM Facilities WHERE Facility_id = {facilityId}";
            Console.WriteLine($"SQL applied: {deleteCommandString}");

            SqlCommand command = new SqlCommand(deleteCommandString, connection);
            //:ExecuteNonQuery(): Used for executing SQL statements that do not return rows, such as INSERT, UPDATE, DELETE
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            return numberOfRowsAffected;
        }

        private int UpdateFacility(SqlConnection connection, Facilities facility) {
            Console.WriteLine("Calling -> UpdateFacility");

            string updateCommandString = $"UPDATE Facilities SET Facility_Name='{facility.Facility_Name}' WHERE Facility_id = {facility.Facility_id}";
            Console.WriteLine($"SQL applied: {updateCommandString}");

            SqlCommand command = new SqlCommand(updateCommandString, connection);
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            return numberOfRowsAffected;
        }

        private int InsertFacility(SqlConnection connection, Facilities facility) {
            Console.WriteLine("Calling -> InsertFacility");

            string insertCommandString = $"INSERT INTO Facilities VALUES({facility.Facility_id}, '{facility.Facility_Name}')";
            Console.WriteLine($"SQL applied: {insertCommandString}");

            SqlCommand command = new SqlCommand(insertCommandString, connection);
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            return numberOfRowsAffected;
        }

        private List<Facilities> ListAllFacilities(SqlConnection connection) {
            Console.WriteLine("Calling -> ListAllFacilities");

            string queryStringAllFacilities = "SELECT * FROM Facilities";
            Console.WriteLine($"SQL applied: {queryStringAllFacilities}");

            SqlCommand command = new SqlCommand(queryStringAllFacilities, connection);
            SqlDataReader reader = command.ExecuteReader();

            List<Facilities> facilitiesList = new List<Facilities>();
            while (reader.Read())
            {
                Facilities facility = new Facilities()
                {
                    Facility_id = reader.GetInt32(0),
                    Facility_Name = reader.GetString(1)
                };
                facilitiesList.Add(facility);
                Console.WriteLine(facility);
            }

            reader.Close();
            return facilitiesList;
        }

        private Facilities GetFacility(SqlConnection connection, int facilityId) {
            Console.WriteLine("Calling -> GetFacility");

            string queryStringOneFacility = $"SELECT * FROM Facilities WHERE Facility_id = {facilityId}";
            Console.WriteLine($"SQL applied: {queryStringOneFacility}");

            SqlCommand command = new SqlCommand(queryStringOneFacility, connection);
            SqlDataReader reader = command.ExecuteReader();

            Facilities facility = null;
            if (reader.Read())
            {
                facility = new Facilities()
                {
                    Facility_id = reader.GetInt32(0),
                    Facility_Name = reader.GetString(1)
                };
                Console.WriteLine(facility);
            }

            reader.Close();
            return facility;
        }

        public void Start() {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // List all facilities
                ListAllFacilities(connection);

                // Create a new facility with Facility_id equal to current max + 1
                Facilities newFacility = new Facilities()
                {
                    Facility_id = GetMaxFacilityId(connection) + 1,
                    Facility_Name = "New Facility"
                };

                // Insert the new facility into the database
                InsertFacility(connection, newFacility);

                // List all facilities including the newly inserted one
                ListAllFacilities(connection);

                // Get the newly inserted facility for updating
                Facilities facilityToBeUpdated = GetFacility(connection, newFacility.Facility_id);

                // Update the Facility_Name
                facilityToBeUpdated.Facility_Name += " (Updated)";

                // Update the facility in the database
                UpdateFacility(connection, facilityToBeUpdated);

                // List all facilities including the updated one
                ListAllFacilities(connection);

                // Get the updated facility for deletion
                Facilities facilityToBeDeleted = GetFacility(connection, facilityToBeUpdated.Facility_id);

                // Delete the facility
                DeleteFacility(connection, facilityToBeDeleted.Facility_id);

                // List all facilities after deletion
                ListAllFacilities(connection);
            }
        }
    }
}
