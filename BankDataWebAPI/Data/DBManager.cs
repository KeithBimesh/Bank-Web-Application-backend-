using System.Data.SQLite;
using BankDataWebAPI.Models;
using BankDataWebAPI.Passwords;

namespace BankDataWebAPI.Data
{
    public class DBManager
    {
        private static string connectionString = "Data Source=bankdatabase.db;Version=3";

        public static void DBInitialize()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        PRAGMA foreign_keys = ON;
                        
                        CREATE TABLE IF NOT EXISTS Users
                        (
                            ID INTEGER PRIMARY KEY AUTOINCREMENT,
                            UserName TEXT NOT NULL UNIQUE,
                            Name TEXT NOT NULL,
                            Email TEXT NOT NULL UNIQUE,
                            Address TEXT,
                            Phone TEXT,
                            PicturePath TEXT,
                            PasswordHashed TEXT NOT NULL,
                            PasswordSalted TEXT NOT NULL,
                            IsActive INTEGER NOT NULL DEFAULT 1
                        );

                        CREATE TABLE IF NOT EXISTS Accounts
                        (
                            ID INTEGER PRIMARY KEY AUTOINCREMENT,
                            UserID INTEGER NOT NULL,
                            AccountNumber TEXT NOT NULL UNIQUE,
                            AccountType TEXT NOT NULL,
                            Balance DECIMAL NOT NULL DEFAULT 0,
                            IsActive INTEGER NOT NULL DEFAULT 1,

                            FOREIGN KEY (UserID)
                            REFERENCES Users(ID)
                        );

                        CREATE TABLE IF NOT EXISTS Transactions
                        (
                            ID INTEGER PRIMARY KEY AUTOINCREMENT,
                            AccountID INTEGER NOT NULL,
                            RelatedAccountID INTEGER,
                            TransactionType TEXT NOT NULL,
                            Amount DECIMAL NOT NULL,
                            TransactionDate TEXT NOT NULL,

                            FOREIGN KEY (AccountID)
                            REFERENCES Accounts(ID),

                            FOREIGN KEY (RelatedAccountID)
                            REFERENCES Accounts(ID)
                       ); ";

                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Database initialized successfully");
            SeedData();
        }

        public static bool InsertUser(User user)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            INSERT INTO Users
                            (UserName, Name, Email, Address, Phone, PicturePath, PasswordHashed, PasswordSalted, IsActive)
                            VALUES
                            (@UserName, @Name, @Email, @Address, @Phone, @PicturePath, @PasswordHashed, @PasswordSalted, @IsActive)";

                        command.Parameters.AddWithValue("@UserName", user.UserName);
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Address", user.Address);
                        command.Parameters.AddWithValue("@Phone", user.Phone);
                        command.Parameters.AddWithValue("@PicturePath", user.PicturePath);
                        command.Parameters.AddWithValue("@PasswordHashed", user.PasswordHashed);
                        command.Parameters.AddWithValue("@PasswordSalted", user.PasswordSalted);
                        command.Parameters.AddWithValue("@IsActive", user.IsActive);

                        int rowsInserted = command.ExecuteNonQuery();

                        if(rowsInserted > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

        public static User getUserByUserName(string username)
        {
            User user = null;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            SELECT ID, UserName, Name, Email, Address, Phone, PicturePath, PasswordHashed, PasswordSalted, IsActive
                            FROM Users
                            WHERE UserName = @UserName";

                        command.Parameters.AddWithValue("@UserName", username);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                user = new User();

                                user.ID = Convert.ToInt32(reader["ID"]);
                                user.UserName = reader["UserName"].ToString();
                                user.Name = reader["Name"].ToString();
                                user.Email = reader["Email"].ToString();
                                user.Address = reader["Address"].ToString();
                                user.Phone = reader["Phone"].ToString();
                                user.PicturePath = reader["PicturePath"].ToString();
                                user.PasswordHashed = reader["PasswordHashed"].ToString();
                                user.PasswordSalted = reader["PasswordSalted"].ToString();
                                user.IsActive = Convert.ToBoolean(reader["IsActive"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return user;
        }

        public static User GetUserByID(int id)
        {
            User user = null;

            try
            {
                using(SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using(SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                           SELECT ID, UserName, Name, Email, Address, Phone, PicturePath,
                                  PasswordHashed, PasswordSalted, IsActive
                           FROM Users
                           WHERE ID = @ID";

                        command.Parameters.AddWithValue("@ID", id);

                        using(SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                user = new User();

                                user.ID = Convert.ToInt32(reader["ID"]);
                                user.UserName = reader["UserName"].ToString();
                                user.Name = reader["Name"].ToString();
                                user.Email = reader["Email"].ToString();
                                user.Address = reader["Address"].ToString();
                                user.Phone = reader["Phone"].ToString();
                                user.PicturePath = reader["PicturePath"].ToString();
                                user.PasswordHashed = reader["PasswordHashed"].ToString();
                                user.PasswordSalted = reader["PasswordSalted"].ToString();
                                user.IsActive = Convert.ToBoolean(reader["IsActive"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return user;
        }

        public static User GetUserByEmail(string email)
        {
            User user = null;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            SELECT ID, UserName, Name, Email, Address, Phone, PicturePath, PasswordHashed, PasswordSalted, IsActive
                            FROM Users
                            WHERE Email = @Email";

                        command.Parameters.AddWithValue("@Email", email);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                user = new User();

                                user.ID = Convert.ToInt32(reader["ID"]);
                                user.UserName = reader["UserName"].ToString();
                                user.Name = reader["Name"].ToString();
                                user.Email = reader["Email"].ToString();
                                user.Address = reader["Address"].ToString();
                                user.Phone = reader["Phone"].ToString();
                                user.PicturePath = reader["PicturePath"].ToString();
                                user.PasswordHashed = reader["PasswordHashed"].ToString();
                                user.PasswordSalted = reader["PasswordSalted"].ToString();
                                user.IsActive = Convert.ToBoolean(reader["IsActive"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return user;
        }

        public static bool UpdateUser(User user)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            UPDATE Users
                            SET Name = @Name,
                                Email = @Email,
                                Address = @Address,
                                Phone = @Phone,
                                PicturePath = @PicturePath
                            WHERE ID = @ID AND IsActive = 1";

                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Address", user.Address);
                        command.Parameters.AddWithValue("@Phone", user.Phone);
                        command.Parameters.AddWithValue("@PicturePath", user.PicturePath);
                        command.Parameters.AddWithValue("@ID", user.ID);

                        int rowsUpdated = command.ExecuteNonQuery();

                        if (rowsUpdated > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return false;
        }

        public static bool DeactivateUser(int id)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            UPDATE Users
                            SET IsActive = 0
                            WHERE ID = @ID";

                        command.Parameters.AddWithValue("@ID", id);

                        int rowsUpdated = command.ExecuteNonQuery();

                        if(rowsUpdated > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

        public static bool InsertAccount(Accounts accounts)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            INSERT INTO Accounts(UserID, AccountNumber, AccountType, Balance, IsActive)
                            VALUES
                            (@UserID, @AccountNumber, @AccountType, @Balance, @IsActive)";

                        command.Parameters.AddWithValue("@UserID", accounts.UserID);
                        command.Parameters.AddWithValue("@AccountNumber", accounts.AccountNumber);
                        command.Parameters.AddWithValue("@AccountType", accounts.AccountType);
                        command.Parameters.AddWithValue("@Balance", accounts.Balance);
                        command.Parameters.AddWithValue("IsActive", accounts.IsActive);

                        int rowsInserted = command.ExecuteNonQuery();

                        if(rowsInserted > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

        public static Accounts GetAccountByNumber(string accountNumber)
        {
            Accounts accounts = null;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            SELECT ID, UserID, AccountNumber, AccountType, Balance, IsActive
                            FROM Accounts
                            WHERE AccountNumber = @AccountNumber";

                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                accounts = new Accounts();

                                accounts.ID = Convert.ToInt32(reader["ID"]);
                                accounts.UserID = Convert.ToInt32(reader["UserID"]);
                                accounts.AccountNumber = reader["AccountNumber"].ToString();
                                accounts.AccountType = reader["AccountType"].ToString();
                                accounts.Balance = Convert.ToDecimal(reader["Balance"]);
                                accounts.IsActive = Convert.ToBoolean(reader["IsActive"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return accounts;
        }

        public static bool UpdateAccount(Accounts accounts)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            UPDATE Accounts
                            SET AccountType = @AccountType,
                                Balance = @Balance
                            WHERE ID = @ID";

                        command.Parameters.AddWithValue("@AccountType", accounts.AccountType);
                        command.Parameters.AddWithValue("@Balance", accounts.Balance);
                        command.Parameters.AddWithValue("@ID", accounts.ID);

                        int rowsUpdated = command.ExecuteNonQuery();

                        if(rowsUpdated > 0)
                        {
                            return true;
                        }
                    }
                }          
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

        public static bool DeactivateAccount(int id)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            UPDATE Accounts
                            SET IsActive = 0
                            WHERE ID = @ID";

                        command.Parameters.AddWithValue("@ID", id);

                        int rowsUpdated = command.ExecuteNonQuery();

                        if (rowsUpdated > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

        public static bool InsertTransaction(Transaction transaction)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            INSERT INTO Transactions(AccountID, RelatedAccountID, TransactionType, Amount, TransactionDate)
                            VALUES
                            (@AccountID, @RelatedAccountID, @TransactionType, @Amount, @TransactionDate)";

                        command.Parameters.AddWithValue("@AccountID", transaction.AccountID);

                        command.Parameters.AddWithValue("@RelatedAccountID",
                            transaction.RelatedAccountID.HasValue
                                ? (object)transaction.RelatedAccountID.Value
                                : DBNull.Value);

                        command.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);

                        command.Parameters.AddWithValue("@Amount", transaction.Amount);
                        
                        command.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate.ToString("o"));

                        int rowsInserted = command.ExecuteNonQuery();

                        if(rowsInserted > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }
        public static List<Transaction> GetTransactionsByAccountID(int accountID)
        {
            List<Transaction> transactions = new List<Transaction>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using(SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                          SELECT ID, AccountID, Amount, TransactionDate
                          FROM Transactions
                          WHERE AccountID = @AccountID
                          ORDER BY TransactionDate DESC";

                        command.Parameters.AddWithValue("@AccountID", accountID);

                        using(SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                Transaction transaction = new Transaction();

                                transaction.ID = Convert.ToInt32(reader["ID"]);
                                transaction.AccountID = Convert.ToInt32(reader["AccountID"]);

                                if (reader["RelatedAccountID"] != DBNull.Value)
                                {
                                    transaction.RelatedAccountID = Convert.ToInt32(reader["RelatedAccountID"]);
                                }

                                transaction.TransactionType = reader["TransactionType"].ToString();

                                transaction.Amount = Convert.ToDecimal(reader["Amount"]);

                                transaction.TransactionDate = DateTime.Parse(reader["TransactionDate"].ToString());

                                transactions.Add(transaction);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return transactions;
        }
        public static bool Deposit(int accountID, decimal amount)
        {
            if(amount <= 0)
            {
                return false;
            }
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using(SQLiteTransaction dbTransaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand command = connection.CreateCommand())
                        {
                            command.Transaction = dbTransaction;

                            command.CommandText = @"
                               UPDATE Accounts
                               SET Balance = Balance + @Amount
                               WHERE ID = @AccountID AND IsActive = 1";

                            command.Parameters.AddWithValue("@Amount", amount);
                            command.Parameters.AddWithValue("@AccountID", accountID);

                            int rowsUpdated = command.ExecuteNonQuery();

                            if(rowsUpdated == 0)
                            {
                                dbTransaction.Rollback();
                                return false;
                            }
                        }
                        using (SQLiteCommand command = connection.CreateCommand())
                        {
                            command.Transaction = dbTransaction;

                            command.CommandText = @"
                              INSERT INTO Transactions
                              (AccountID, RelatedAccountID, TransactionType, Amount, TransactionDate)
                              VALUES
                              (@AccountID, NULL, 'Deposit', @Amount, @Date)";

                            command.Parameters.AddWithValue("@AccountID", accountID);
                            command.Parameters.AddWithValue("@Amount", amount);
                            command.Parameters.AddWithValue("@Date", DateTime.UtcNow.ToString("o"));
                            command.ExecuteNonQuery();
                        }
                        dbTransaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        Console.WriteLine("Deposit error: " + ex.Message);
                        return false;
                    }
                }
            }
        }
        public static bool Withdraw(int accountID, decimal amount)
        {
            if(amount <= 0)
            {
                return false;
            }
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteTransaction dbTransaction = connection.BeginTransaction())
                {
                    try
                    {
                        using(SQLiteCommand command = connection.CreateCommand())
                        {
                            command.Transaction = dbTransaction;

                            command.CommandText = @"
                              UPDATE Accounts
                              SET Balance = Balance - @Amount
                              WHERE ID = @AccountID
                              AND IsActive = 1
                              AND Balance >= @Amount";

                            command.Parameters.AddWithValue("@Amount", amount);
                            command.Parameters.AddWithValue("@AccountID", accountID);

                            int rowsUpdated = command.ExecuteNonQuery();

                            if(rowsUpdated == 0)
                            {
                                dbTransaction.Rollback();
                                return false;
                            }
                        }
                        using (SQLiteCommand command = connection.CreateCommand())
                        {
                            command.Transaction = dbTransaction;

                            command.CommandText = @"
                              INSERT INTO Transactions
                              (AccountID, RelatedAccountID, TransactionType, Amount, TransactionDate)
                              VALUES
                              (@AccountID, NULL, 'Withdrawal', @Amount, @Date)";

                            command.Parameters.AddWithValue("@AccountID", accountID);
                            command.Parameters.AddWithValue("@Amount", amount);
                            command.Parameters.AddWithValue("@Date", DateTime.UtcNow.ToString("o"));
                            command.ExecuteNonQuery();
                        }
                        dbTransaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        Console.WriteLine("Withdrawal error: " + ex.Message);
                        return false;
                    }
                }
            }
        }
        public static bool Transfer(int senderAccountID, int recieverAccountID, decimal amount)
        {
            if(amount <= 0 || senderAccountID == recieverAccountID)
            {
                return false;
            }
            using(SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using(SQLiteTransaction dbTransaction = connection.BeginTransaction())
                {
                    try
                    {
                        using(SQLiteCommand command = connection.CreateCommand())
                        {
                            command.Transaction = dbTransaction;

                            command.CommandText = @"
                              UPDATE Accounts
                              SET Balance = Balance - @Amount
                              WHERE ID = @SenderID
                              AND IsActive = 1
                              AND Balance >= @Amount";

                            command.Parameters.AddWithValue("@Amount", amount);
                            command.Parameters.AddWithValue("@SenderID", senderAccountID);

                            int rowsUpdated = command.ExecuteNonQuery();

                            if(rowsUpdated == 0)
                            {
                                dbTransaction.Rollback();
                                return false;
                            }
                        }
                        using(SQLiteCommand command = connection.CreateCommand())
                        {
                            command.Transaction = dbTransaction;

                            command.CommandText = @"
                              UPDATE Accounts
                              SET Balance = Balance + @Amount
                              WHERE ID = @RecieverID
                              AND IsActive = 1";

                            command.Parameters.AddWithValue("@Amount", amount);
                            command.Parameters.AddWithValue("@RecieverID", recieverAccountID);

                            int rowsUpdated = command.ExecuteNonQuery();

                            if(rowsUpdated == 0)
                            {
                                dbTransaction.Rollback();
                                return false;
                            }
                        }
                        using(SQLiteCommand command = connection.CreateCommand())
                        {
                            command.Transaction = dbTransaction;

                            command.CommandText = @"
                              INSERT INTO Transactions
                              (AccountID, RelatedAccountID, TransactionType, Amount, TransactionDate)
                              VALUES
                              (@SenderID, @RecieverID, 'Transfer', @Amount, @Date)";

                            command.Parameters.AddWithValue("@SenderID", senderAccountID);
                            command.Parameters.AddWithValue("@RecieverID", recieverAccountID);
                            command.Parameters.AddWithValue("@Amount", amount);
                            command.Parameters.AddWithValue("@Date", DateTime.UtcNow.ToString("o"));
                            command.ExecuteNonQuery();
                        }
                        dbTransaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        Console.WriteLine("Transfer error: " + ex.Message);
                        return false;
                    }
                }
            }
        }
        public static void SeedData()
        {
            try
            {
                for(int i = 1; i <= 20; i++)
                {
                    string username = "SeedUser" + i;

                    if(getUserByUserName(username) == null)
                    {
                        string passwordHash;
                        string passwordSalt;

                        PasswordManager.CreatePasswordHash("Bank123", out passwordHash, out passwordSalt);

                        User user = new User
                        {
                            UserName = username,
                            Name = "Sample User " + i,
                            Email = username + "@gmail.com",
                            Address = "Sri lanka",
                            Phone = "071234" + i.ToString("D4"),
                            PicturePath = null,
                            PasswordHashed = passwordHash,
                            PasswordSalted = passwordSalt,
                            IsActive = true
                        };
                        InsertUser(user);
                    }
                }

                for(int i = 1; i <= 30; i++)
                {
                    string accountNumber = "SEED" + i.ToString("D4");

                    if (GetAccountByNumber(accountNumber) == null)
                    {
                        int userNumber = ((i - 1) % 20) + 1;

                        User user = getUserByUserName("SeedUser" + userNumber);

                        if (user == null)
                        {
                            continue;
                        }

                        Accounts account = new Accounts
                        {
                            UserID = user.ID,
                            AccountNumber = accountNumber,
                            AccountType = i % 2 == 0 ? "savings" : "Current",
                            Balance = 0,
                            IsActive = true
                        };
                        InsertAccount(account);
                    }
                }
                List<int> accountIDs = new List<int>();

                using(SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using(SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                           SELECT ID
                           FROM Accounts
                           WHERE AccountNumber LIKE 'SEED%'
                           ORDER BY AccountNumber";

                        using(SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                accountIDs.Add(Convert.ToInt32(reader["ID"]));
                            }
                        }
                    }
                }
                if(accountIDs.Count == 0)
                {
                    Console.WriteLine("Seed accounts can't be created");
                    return;
                }

                foreach(int accountID in accountIDs)
                {
                    decimal balance = 0;

                    using(SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        using(SQLiteCommand command = connection.CreateCommand())
                        {
                            command.CommandText = @"
                              SELECT Balance
                              FROM Accounts
                              WHERE ID = @ID";

                            command.Parameters.AddWithValue("@ID", accountID);

                            object result = command.ExecuteScalar();

                            if(result != null && result != DBNull.Value)
                            {
                                balance = Convert.ToDecimal(result);
                            }
                        }
                    }
                    if(balance == 0)
                    {
                        Deposit(accountID, 10000);
                    }
                }

                int transactionCount;

                using(SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using(SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT COUNT(*) FROM Transactions";

                        transactionCount = Convert.ToInt32(command.ExecuteScalar());
                    }
                }

                int index = 0;

                while(transactionCount < 500)
                {
                    int sourceIndex = index % accountIDs.Count;
                    int destinationIndex = (sourceIndex + 1) % accountIDs.Count;
                    int sourceID = accountIDs[sourceIndex];
                    int destinationID = accountIDs[destinationIndex];

                    bool success;

                    if(index % 5 == 0)
                    {
                        success = Withdraw(sourceID, 10);

                        if(!success)
                        {
                            success = Deposit(sourceID, 25);
                        }
                    }
                    else if(index % 5 == 1)
                    {
                        success = Transfer(sourceID, destinationID, 10);

                        if(!success)
                        {
                            success = Deposit(sourceID, 25);
                        }
                    }
                    else
                    {
                        success = Deposit(sourceID, 25);
                    }
                    if(success)
                    {
                        transactionCount++;
                    }
                    else
                    {
                        Console.WriteLine("Can't create a sample transaction");
                        break;
                    }
                    index++;
                }
                Console.WriteLine("Database seeding completed");
                Console.WriteLine("Sample accounts: " + accountIDs.Count);
                Console.WriteLine("Transactions: " + transactionCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Seeding error: " + ex.Message);
            }
        }

    }
}
