using System;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using ConsoleAppVisuals;
using ConsoleAppVisuals.AnimatedElements;
using ConsoleAppVisuals.Enums;
using ConsoleAppVisuals.InteractiveElements;
using ConsoleAppVisuals.PassiveElements;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using ZstdSharp.Unsafe;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Window.Open();
            Window.Clear();
            MySqlConnection? myConnexion = null;
            try
            {
                (string username, string password) = UserConnection();
                myConnexion = new MySqlConnection(
                    $"Server=localhost;PORT=3306;Database=velomax;Uid={username};Pwd={password};"
                );
                myConnexion.Open();
                if (username == "bozo" && password == "bozo")
                {
                    MenuBozo(myConnexion);
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.Message);
                Console.ReadKey();
            }
            if (myConnexion == null)
            {
                List<string> lines = new List<string> {"Erreur de connexion à la base de donnée, presser entré"};
                Text errorText = new Text(lines:lines, placement : Placement.TopCenter);
                Window.AddElement(errorText);
                Window.ActivateElement(errorText);
                Console.ReadKey();
                Window.RemoveElement(errorText);
                Window.Clear();
                return;
            }

            while (true)
            {
                Window.Clear();
                string[] options = new string[]
                {
                    "Analyse statistique",
                    "Ajouter une valeur",
                    "Afficher les valeurs",
                    "Modifier des valeurs",
                    "Commande union",
                    "Afficher stock",
                    "Supprimer des valeurs",
                    "Démonstration de l'application",
                    "Quitter"
                };
                ScrollingMenu menu = new ScrollingMenu(
                    "Que voulez vous faire",
                    0,
                    Placement.TopCenter,
                    options
                );
                Window.RemoveAllElements();
                Window.AddElement(menu);
                Window.ActivateElement(menu);
                var response = menu.GetResponse();
                switch (response!.Value)
                {
                    case 0:
                        AnalyseStatistique(myConnexion);
                        break;
                    case 1:
                        string table3 = MenuSelector(
                            FindTable(myConnexion),
                            "Choisissez une table"
                        );
                        InsertValue(table3, myConnexion);
                        Console.ReadKey();
                        break;

                    case 2:
                        string table = MenuSelector(FindTable(myConnexion), "Choisissez une table");
                        string condition = "";
                        string ordre = "";
                        string join = "";
                        List<string> listeTable = new List<string>();
                        bool firstcondition = true;
                        bool continuer = true;
                        while (continuer)
                        {
                            if (!listeTable.Contains(table))
                                listeTable.Add(table);
                            AfficherValue(table, myConnexion, condition, ordre, join);
                            int nouvelleVue;
                            ScrollingMenu modificationSelector = new ScrollingMenu(
                                "Que voulez vous ajouter comme condition pour changer le point de vue de la table  ? ",
                                0,
                                Placement.BottomCenterFullWidth,
                                new string[]
                                {
                                    "Condition",
                                    "Ordre de rangement",
                                    "Condition de jointure",
                                    "Quitter"
                                }
                            );
                            Window.AddElement(modificationSelector);
                            Window.ActivateElement(modificationSelector);
                            var response2 = modificationSelector.GetResponse();
                            nouvelleVue = response2!.Value;
                            Window.RemoveElement(modificationSelector);
                            switch (nouvelleVue)
                            {
                                case 0:
                                    string tableCondition = table;
                                    if (listeTable.Count > 1)
                                    {
                                        Console.WriteLine(
                                            "Sur quelle table voulez vous mettre la condition ?"
                                        );
                                        tableCondition = MenuSelector(
                                            listeTable.ToArray(),
                                            "Choisissez une table"
                                        );
                                    }
                                    if (!firstcondition)
                                    {
                                        condition += MenuSelector(
                                            new string[] { " AND ", " OR " },
                                            "Voulez vous mettre un AND ou un OR ? ",
                                            Placement.TopRight
                                        );
                                    }
                                    condition += Condition(
                                        tableCondition,
                                        myConnexion,
                                        firstcondition
                                    );
                                    firstcondition = false;
                                    Window.Clear();
                                    break;
                                case 1:
                                    ordre = Order(table, myConnexion);
                                    Window.Clear();
                                    break;
                                case 2:
                                    foreach (string var in listeTable)
                                    {
                                        Console.WriteLine(var);
                                        Console.ReadKey();
                                    }
                                    string[] joinTable = Join(listeTable.ToArray(), myConnexion);
                                    join = joinTable[1];
                                    if (join != "")
                                        listeTable.Add(joinTable[0]);
                                    break;
                                case 3:
                                    continuer = false;
                                    Window.Clear();
                                    break;
                            }
                            Window.RemoveAllElements();
                            Window.Clear();
                            if (!continuer)
                            {
                                break;
                            }
                        }
                        break;

                    case 3:
                        string table4 = MenuSelector(
                            FindTable(myConnexion),
                            "Choisissez une table :"
                        );
                        ModifyValue(table4, myConnexion);
                        break;
                    case 4:
                        string table7 = MenuSelector(
                            FindTable(myConnexion),
                            "Choisissez une table :"
                        );
                        string table8 = MenuSelector(
                            FindTable(myConnexion),
                            "Choisissez une table :"
                        );
                        Union(table7, table8, myConnexion);
                        Console.ReadKey();
                        break;
                    case 5:
                        AffichageStock(myConnexion);
                        break;
                    case 6:
                        string table5 = MenuSelector(
                            FindTable(myConnexion),
                            "Choisissez une table :"
                        );
                        SupprimerValue(table5, myConnexion);
                        break;
                    case 7:
                        Demonstration(myConnexion);
                        break;
                    case 8:
                        myConnexion.Close();
                        Window.Close();
                        break;
                }
            }
        }

        /// <summary>
        /// The menu associated with the user bozo
        /// </summary>
        /// <param name="myConnexion">Connection avec la base</param>
        public static void MenuBozo(MySqlConnection myConnexion)
        {
            while (true)
            {
                ScrollingMenu scrollingMenu = new ScrollingMenu(
                    "Que voulez vous faire ?",
                    0,
                    Placement.TopCenter,
                    new string[] { "Voir les stockes", "Voir la ligne d'assemblage", "Quitter" }
                );
                Window.AddElement(scrollingMenu);
                Window.ActivateElement(scrollingMenu);
                var response = scrollingMenu.GetResponse();

                switch (response!.Value)
                {
                    case 0:
                        AffichageStock(myConnexion);
                        Console.ReadKey();
                        break;
                    case 1:
                        AfficherValue("_ligneassemblage", myConnexion);
                        Console.Write("En maintenance cette fonctionnalité n'est pas encore disponible");
                        Console.ReadKey();
                        break;
                    case 2:
                        Window.Clear();
                        return;
                }
                Window.Clear();
            }
        }

        /// <summary>
        /// Cette fonction permet de savoir si une colonne est une clé étrangère
        /// </summary>
        /// <param name="username">unsername de l'utilisateur</param>
        /// <param name="password">mot de passe de l'utilisateur</param>
        /// <returns></returns>
        public static bool CheckUserExists(string username, string password)
        {
            string connectionString =
                $"Server=localhost;PORT=3306;Database=velomax;Uid={username};Pwd={password};";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (MySqlException e )
            {

                return false;
            }
        }

        static (string username, string password) UserConnection()
        {
            while (true)
            {
                Prompt username = new Prompt("Enter your username:", "", Placement.TopCenter);
                Window.AddElement(username);
                Window.ActivateElement(username);
                var response = username.GetResponse()!.Value;
                Window.RemoveElement(username);
                Prompt password = new Prompt("Enter your password:", "", Placement.TopCenter);
                Window.AddElement(password);
                Window.ActivateElement(password);
                var response2 = password.GetResponse()!.Value;
                Window.RemoveElement(password);
                if (CheckUserExists(response, response2))

                    return (response, response2);
                else
                {
                    List<string> lines = new List<string> {"Erreur de connexion à la base de donnée, presser entré"};
                    Text errorText = new Text(lines:lines, placement : Placement.TopCenter);
                    Window.AddElement(errorText);
                    Window.ActivateElement(errorText);
                    Console.ReadKey();
                    Window.RemoveElement(errorText);
                    Window.Clear();
                }
            }
        }

        /// <summary>
        /// Create a column in a table
        /// </summary>
        /// <param name="table">table que l'on utilise dans la base</param>
        /// <param name="column">colonne que l'on utilise dans la table</param>
        /// <param name="myConnexion">connexion avec la base</param>
        public static void CreateColumn(string table, MySqlConnection myConnexion)
        {
            try
            {
                //We select the name of the new column
                Prompt promptColumn = new Prompt(
                    "Enter the name of the column:",
                    "",
                    Placement.TopCenter
                );
                Window.AddElement(promptColumn);
                Window.ActivateElement(promptColumn);
                var column = promptColumn.GetResponse()!.Value;
                Window.RemoveElement(promptColumn);

                //We select the type of the new column
                string type = MenuSelector(
                    new string[] { "INT", "VARCHAR(100)", "DATE" },
                    "Choisissez le type de la colonne"
                );
                //myConnexion.Open();
                MySqlCommand myCommand = new MySqlCommand(
                    $"ALTER TABLE {table} ADD COLUMN {column} {type}",
                    myConnexion
                );
                myCommand.ExecuteNonQuery();
                Console.WriteLine("Colonne ajoutée");
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.Message);
            }
        }

        /// <summary>
        /// Insert une nouvelle valeur dans une table
        /// </summary>
        /// <param name="table">table dans laquelle on ajoute une valeur</param>
        /// <param name="myConnexion">connexion avec la base SQL</param>
        public static void InsertValue(string table, MySqlConnection myConnexion)
        {
            // On récupère les colonnes de la table
            string[] listeColumn = FindColonne(table, myConnexion);
            string lcolone = "";
            string lvalue = "";
            for (int i = 0; i < listeColumn.Length; i++)
            {
                if (listeColumn[i] != "")
                {
                    Prompt prompt = new Prompt(
                        $"Enter the value of the column {listeColumn[i]} (type : {GetColumnType(table, listeColumn[i], myConnexion)}):",
                        "",
                        Placement.TopCenter
                    );
                    Window.AddElement(prompt);
                    Window.ActivateElement(prompt);
                    var response = prompt.GetResponse()!.Value;
                    if (IsPrimaryKey(table, listeColumn[i], myConnexion))
                    {
                        if (response == "")
                        {
                            Console.WriteLine("Vous ne pouvez pas mettre une clé primaire à null");
                            Console.ReadKey();
                            return;
                        }
                        if (ValueExists(table, listeColumn[i], response, myConnexion))
                        {
                            Console.WriteLine("La valeur existe déjà");
                            Console.ReadKey();
                            return;
                        }
                    }
                    else if (IsForeignKey(table, listeColumn[i], myConnexion))
                    {
                        (string refTable, string refColonne) = GetForeignKeyReference(
                            table,
                            listeColumn[i],
                            myConnexion
                        );
                        if (!ValueExists(refTable, refColonne, response, myConnexion))
                        {
                            Console.WriteLine(
                                "Vous ne pouvez pas mettre une clé étrangère qui n'existe pas dans la table parente"
                            );
                            Console.ReadKey();
                            break;
                        }
                    }
                    if (response == "")
                    {
                        Console.WriteLine("Vous ne pouvez pas mettre une valeur à null");
                        i--;
                        Console.ReadKey();
                    }
                    if (response != "")
                    {
                        lcolone += listeColumn[i] + ",";
                        lvalue += $"'{response}',";
                    }
                }
            }
            // On enlève la dernière virgule
            if (lcolone != "" && lvalue != "")
            {
                lcolone = lcolone.Substring(0, lcolone.Length - 1);
                lvalue = lvalue.Substring(0, lvalue.Length - 1);
            }
            // On exécute la commande SQL
            MySqlCommand myCommand = new MySqlCommand(
                $"INSERT INTO {table} ({lcolone}) VALUES ({lvalue})",
                myConnexion
            );
            myCommand.ExecuteNonQuery();
            Console.WriteLine("Valeur ajoutée");
        }

        /// <summary>
        /// Cette fonction permet de savoir si une colonne est une clé étrangère
        /// </summary>
        /// <param name="tableName">Table que l'on utilise</param>
        /// <param name="columnName">Colonne dans la table qu'on utilise</param>
        /// <param name="connection">Connection avec la base</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (string ReferencedTable, string ReferencedColumn) GetForeignKeyReference(
            string tableName,
            string columnName,
            MySqlConnection connection
        )
        {
            string query =
                @"
                SELECT REFERENCED_TABLE_NAME, REFERENCED_COLUMN_NAME
                FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                WHERE TABLE_NAME = @tableName AND COLUMN_NAME = @columnName";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@tableName", tableName);
                cmd.Parameters.AddWithValue("@columnName", columnName);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (reader.GetString(0), reader.GetString(1));
                    }
                    else
                    {
                        throw new Exception("No foreign key reference found.");
                    }
                }
            }
        }

        /// <summary>
        /// Cette fonction permet d'afficher un tableau d'une table sql en fonction des colonnes que l'on veut afficher
        /// </summary>
        /// <param name="table">table que l'on cherche à afficher</param>
        /// <param name="myConnexion">connexion avec la base</param>
        /// <param name="condition">la condition qui a été ajoutée </param>
        /// <param name="order">si il y a un order by dans la requête</param>
        /// <param name="join">si il y a un join dans la requete</param>
        /// <param name="printColone">si une requête précise doit être effectuée </param>
        /// <param name="WantColumns">list des colonnes que l'utilisateur souhaite utiliser </param>
        public static void AfficherValue(
            string table,
            MySqlConnection myConnexion,
            string condition = "",
            string order = "",
            string join = "",
            string printColone = "",
            string[] WantColumns = null
        )
        {
            try
            {
                string[] selectedColumns = new string[FindColonne(table, myConnexion).Length];
                if (printColone == "" || printColone is null)
                {
                    if (condition == "" && order == "" && join == "")
                    {
                        selectedColumns = FindColonne(table, myConnexion);
                    }
                    else
                    {
                        selectedColumns = SelectColumns(table, myConnexion);
                    }
                    for (int i = 0; i < selectedColumns.Length; i++)
                    {
                        if (selectedColumns[i] != "")
                        {
                            printColone += $"{table}.{selectedColumns[i]}" + ",";
                        }
                    }
                    if (printColone is null || printColone == "")
                    {
                        printColone = "*";
                    }
                }
                printColone = printColone.Substring(0, printColone.Length - 1);
                MySqlCommand myCommand = new MySqlCommand(
                    $"SELECT {printColone} FROM {table} {join} {condition} {order}",
                    myConnexion
                );
                if (WantColumns != null)
                {
                    selectedColumns = WantColumns;
                }
                else
                {
                    selectedColumns = FindColonne(table, myConnexion);
                }
                List<string> columns = new List<string>();
                for (int i = 0; i < selectedColumns.Length; i++)
                {
                    if (selectedColumns[i] != "")
                    {
                        columns.Add(selectedColumns[i]);
                    }
                }
                List<List<string>> values = new List<List<string>>();
                MySqlDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    List<string> row = new List<string>();
                    for (int i = 0; i < selectedColumns.Length; i++)
                    {
                        if (selectedColumns[i] != "" && !columns.Contains(selectedColumns[i]))
                        {
                            columns.Add(selectedColumns[i]);
                        }
                        else
                        {
                            row.Add(myReader[selectedColumns[i]].ToString());
                        }
                    }
                    values.Add(row);
                }
                myReader.Close();
                TableView tableView = new TableView($"Values de {table}", columns, values);
                Window.AddElement(tableView);
                Window.ActivateElement(tableView);
                Prompt prompt = new Prompt(
                    "Appuyez sur une touche pour continuer",
                    "",
                    Placement.BottomCenterFullWidth
                );
                Window.AddElement(prompt);
                Window.ActivateElement(prompt);
                //Window.RemoveElement(tableView,prompt);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.Message);
            }
        }

        /// <summary>
        /// Ce programme est fait pour faire une démonstration des commandes SQL demandées
        /// </summary>
        /// <param name="myConnexion">Connexion avec la base de donée</param>
        public static void Demonstration(MySqlConnection myConnexion)
        {
            ScrollingMenu menuDemo = new ScrollingMenu(
                "Quelle démonstration voulez vous faire ?",
                0,
                Placement.TopCenter,
                new string[]
                {
                    "Ajouter un client",
                    "Modifier un client",
                    "Supprimet un client",
                    "Compter le nombre de client",
                    "Noms des clients avec le cumul de toutes ses commandes en euros ",
                    "Liste des produits ayant une quantité en stock <= 2",
                    "Nombres de pièces et/ou vélos fournis par fournisseur. ",
                    "Le chiffre daffaires par magasin",
                    "ventes générées par vendeur",
                    "Quitter"
                }
            );
            Window.AddElement(menuDemo);
            Window.ActivateElement(menuDemo);
            var response = menuDemo.GetResponse();
            Window.RemoveElement(menuDemo);
            switch (response!.Value)
            {
                case 0:
                    string tableClient = "___clientparticulier";
                    // on veut ajouter un client
                    InsertValue(tableClient, myConnexion);
                    AfficherValue(tableClient, myConnexion);
                    break;
                case 1:
                    tableClient = "___clientparticulier";
                    // on veut modifier un client
                    ModifyValue(tableClient, myConnexion);
                    AfficherValue(tableClient, myConnexion);
                    break;
                case 2:
                    tableClient = "___clientparticulier";
                    // on veut supprimer un client
                    SupprimerValue(tableClient, myConnexion);
                    AfficherValue(tableClient, myConnexion);
                    break;
                case 3:
                    tableClient = "___clientparticulier";
                    // on veut compter le nombre de client
                    AfficherValue(
                        table: " (SELECT IdClientParticulier FROM ___ClientParticulier UNION SELECT IdClientPro FROM ____ClientPro) AS ClientsTotal ",
                        myConnexion,
                        printColone: " COUNT(*) AS NombreClients ",
                        WantColumns: new string[] { "NombreClients" }
                    );

                    break;
                case 4:
                    tableClient = "___clientparticulier";
                    // on veut afficher le nom des clients avec le cumul de toutes ses commandes en euros
                    AfficherValue(
                        tableClient,
                        myConnexion,
                        order: "GROUP BY ___clientparticulier.NomClientParticulier",
                        join: "JOIN ___commande_particulier ON ___commande_particulier.IdClientParticulier = ___clientparticulier.IdClientParticulier",
                        printColone: "___clientparticulier.NomClientParticulier, SUM(_commande.PrixTotalCommande) ",
                        WantColumns:
                        [
                            "NomClientParticulier",
                            "SUM(___commande_particulier.PrixTotalCommande)"
                        ]
                    );
                    Console.ReadKey();
                    break;
                case 5:
                    // on veut afficher les produits ayant une quantité en stock <= 2
                    AfficherValue(
                        table: "__stockagepiece",
                        myConnexion,
                        condition: "WHERE quantitePiece <=2",
                        printColone: "IdPiece,quantitePiece ",
                        WantColumns: ["IdPiece", "quantitePiece"]
                    );
                    Console.ReadKey();
                    break;
                case 6:
                    // on veut afficher le nombre de pièces et/ou vélos fournis par fournisseur"GROUP BY __fournisseur.SiretFournisseur "*
                    AfficherValue(
                        table: "__Fournisseur f ",
                        myConnexion,
                        order: "GROUP BY f.NomFournisseur ",
                        join: "LEFT JOIN __Piece p ON f.SiretFournisseur = p.SiretFournisseur ",
                        printColone: "f.NomFournisseur,COUNT(DISTINCT p.IdPiece) AS NombrePiecesFournies ",
                        WantColumns: ["NomFournisseur", "NombrePiecesFournies"]
                    );
                    break;
                case 7:
                    Console.WriteLine("Le chiffre d’affaires par magasin");
                    // on veut afficher le chiffre d'affaires par magasin
                    AfficherValue(
                        table: "__magasin",
                        myConnexion,
                        printColone: "__magasin.NomMagasin, __magasin.ChiffreAffairesMagasin ",
                        WantColumns: ["NomMagasin", "ChiffreAffairesMagasin"]
                    );
                    break;
                case 8:
                    // on veut afficher les ventes générées par vendeur
                    //AfficherValue("_vendeur",myConnexion,"","GROUP BY _vendeur.IdVendeur","JOIN _commande ON _commande.IdVendeur = _vendeur.IdVendeur","_vendeur.NomVendeur, SUM(_commande.PrixTotalCommande) ",["NomVendeur","SUM(_commande.PrixTotalCommande)"]);

                    MySqlCommand mySqlCommand = new MySqlCommand(
                        "SELECT __vendeur.NomVendeur, SUM(_commande_particulier.PrixTotalCommande) FROM __vendeur JOIN _commande ON _commande.IdVendeur = __vendeur.IdVendeur GROUP BY __vendeur.IdVendeur",
                        myConnexion
                    );
                    List<string> columns1 = new List<string>();
                    columns1.Add("NomVendeur");
                    columns1.Add("SUM(_commande.PrixTotalCommande)");

                    List<List<string>> values1 = new List<List<string>>();
                    MySqlDataReader myReader1 = mySqlCommand.ExecuteReader();
                    while (myReader1.Read())
                    {
                        List<string> row = new List<string>();
                        row.Add(myReader1["NomVendeur"].ToString());
                        row.Add(myReader1["SUM(_commande.PrixTotalCommande)"].ToString());
                        values1.Add(row);
                    }
                    myReader1.Close();
                    TableView tableView1 = new TableView($"Values de __vendeur", columns1, values1);
                    myReader1.Close();
                    Window.AddElement(tableView1);
                    Window.ActivateElement(tableView1);
                    Console.ReadKey();

                    break;
            }
        }

        /// <summary>
        /// Cette fonction permet de supprimer une valeur dans une table
        /// </summary>
        /// <param name="table">table que l'on utilise</param>
        /// <param name="column">colonne de la table </param>
        /// <param name="myConnexion">Connection avec la bdd</param>
        /// <returns></returns>
        public static bool IsPrimaryKey(string table, string column, MySqlConnection myConnexion)
        {
            MySqlCommand myCommand = new MySqlCommand(
                $"SELECT COLUMN_KEY FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table}' AND COLUMN_NAME = '{column}'",
                myConnexion
            );
            MySqlDataReader myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                if (myReader["COLUMN_KEY"].ToString() == "PRI")
                {
                    myReader.Close();
                    return true;
                }
            }
            myReader.Close();

            return false;
        }

        public static bool ValueExists(
            string table,
            string column,
            string value,
            MySqlConnection connection
        )
        {
            string query = $"SELECT COUNT(*) FROM {table} WHERE {column} = @value";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@value", value);

                var result = cmd.ExecuteScalar();
                return Convert.ToInt32(result) > 0;
            }
        }

        /// <summary>
        /// Cette fonction permet de compter des valeurs en utilisant count
        /// </summary>
        /// <param name="table"></param>
        /// <param name="myConnexion"></param>
        /// <param name="condition"></param>
        /// <param name="action"></param>
        public static void CompterValeurs(
            string table,
            MySqlConnection myConnexion,
            string condition = "",
            string action = "COUNT"
        )
        {
            try
            {
                //on séléctionne la colonne uqe l'on veut compter
                string column = MenuSelector(
                    FindColonne(table, myConnexion),
                    "Choisissez une colonne que vous souhaitez compter"
                );
                // on séléctionne si oui ou non un group by
                string groupby = "";
                string Select = MenuSelector(
                    new string[] { "Oui", "Non" },
                    "Voulez vous faire un group by ?"
                );

                if (Select == "Oui")
                {
                    groupby = MenuSelector(
                        FindColonne(table, myConnexion),
                        "Choisissez une colonne pour le group by"
                    );
                    // on écris la commande SQL que l'on va exécuter
                    MySqlCommand myCommand = new MySqlCommand(
                        $"SELECT {groupby}, {column}, {action}({column}) FROM {table} {condition} GROUP BY {groupby}",
                        myConnexion
                    );
                    List<string> columns = new List<string>();
                    columns.Add(column);
                    columns.Add(action);
                    List<List<string>> values = new List<List<string>>();
                    MySqlDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        List<string> row = new List<string>();
                        row.Add(myReader[column].ToString());
                        row.Add(myReader[$"{action}(" + column + ")"].ToString());
                        values.Add(row);
                    }
                    myReader.Close();
                    TableView tableView = new TableView($"Values de {table}", columns, values);
                    myReader.Close();
                    Window.AddElement(tableView);
                    Window.ActivateElement(tableView);
                    Console.ReadKey();
                }
                else
                {
                    MySqlCommand myCommand = new MySqlCommand(
                        $"SELECT {action}({column}) FROM {table} {condition}",
                        myConnexion
                    );
                    MySqlDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        Console.WriteLine(
                            $"Il y a {myReader[$"{action}(" + column + ")"]} valeurs dans la colonne {column}"
                        );
                    }
                    myReader.Close();
                    Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.Message);
            }
        }

        /// <summary>
        /// Cette fonction permet d'avoir les différentes statistiques sur une table ( celles demandées dans le projet )
        /// </summary>
        /// <param name="myConnexion">Connexion pour la base SQL</param>
        public static void AnalyseStatistique(MySqlConnection myConnexion)
        {
            // on demande quelle étude il souhaite faire
            ScrollingMenu scrollingMenu = new ScrollingMenu(
                "Que voulez vous étudier ?",
                0,
                Placement.TopCenter,
                new string[]
                {
                    "Quantité vendue",
                    "membre par programme d'adhésion",
                    "date expiration adhésion",
                    "Meilleurs clients",
                    "montants des commandes par vendeur",
                    "bonus des salariés",
                    "nombre moyen de pièces",
                    "Quitter"
                }
            );
            Window.AddElement(scrollingMenu);
            Window.ActivateElement(scrollingMenu);
            int response = scrollingMenu.GetResponse()!.Value;

            switch (response)
            {
                case 0:
                    // pour les quantités vendues on doit faire la sommes de chaque distinct VEND ou l'on a le même produit qu'une pièces
                    //CompterValeurs(string table, MySqlConnection myConnexion,string condition="",string action = "COUNT")//
                    AfficherValue(
                        "__vendeur v ",
                        myConnexion,
                        join: "JOIN __magasin m ON v.IdMagasin = m.IdMagasin LEFT JOIN __stockagevelo sv ON m.IdMagasin = sv.IdMagasin",
                        order: "GROUP BY m.IdMagasin, v.IdVendeur ",
                        printColone: "m.IdMagasin, v.IdVendeur,COALESCE(SUM(sv.quantiteVelo), 0) AS QuantiteTotale ",
                        WantColumns: new string[] { "IdMagasin", "IdVendeur", "QuantiteTotale" }
                    );
                    break;
                case 1:
                    AfficherValue(
                        "adhesion a ",
                        myConnexion,
                        printColone: " a.IdProgramme AS Programme, a.IdClientParticulier AS IdPersonne ",
                        WantColumns: new string[] { "Programme", "IdPersonne" }
                    );
                    break;
                case 2:
                    // aficher les dates de fin des adhésions
                    AfficherValue(
                        "adhesion a ",
                        myConnexion,
                        join: "JOIN __programme p ON a.IdProgramme = p.IdProgramme",
                        printColone: "a.IdProgramme AS Programme, a.IdClientParticulier AS IdPersonne,a.DateFinAdhesion AS DateExpiration ",
                        WantColumns: new string[] { "Programme", "IdPersonne", "DateExpiration" }
                    );
                    break;
                case 3:
                    // on prends le meilleur client en fonction des quantités vendues
                    AfficherValue(
                        "( SELECT ___clientparticulier.IdClientParticulier FROM ___clientparticulier UNION SELECT ____clientPro.IdClientPro FROM ____clientPro ) AS clientPro ",
                        myConnexion,
                        printColone: "COUNT(*) AS NombreClients ",
                        WantColumns: new string[] { "NombreClients" }
                    );
                    break;
                case 4:
                    break;
                case 5:
                    AfficherValue(
                        "__Vendeur v",
                        myConnexion,
                        join: "JOIN __magasin m ON v.IdMagasin = m.IdMagasin;",
                        printColone: "v.IdVendeur, v.NomVendeur, v.StatutVendeur,(ChiffreAffairesMagasin * (SatistificationClient / 100)) AS Bonus ",
                        WantColumns: new string[]
                        {
                            "IdVendeur",
                            "NomVendeur",
                            "StatutVendeur",
                            "Bonus"
                        }
                    );
                    break;
                case 6:
                    // on veut afficher le nombre moyen de pièces
                    AfficherValue(
                        printColone: "AVG(quantitePiece) AS NombreMoyenPieces ",
                        WantColumns: new string[] { "NombreMoyenPieces" },
                        myConnexion:myConnexion,
                        table:"__stockagepiece"
                    );
                    Console.ReadKey();

                    //"bonus des salariés
                    break;
            }
        }

        /// <summary>
        /// Cette fonction permet d'afficher le stocke général d'un magasin, pièce etc...
        /// </summary>
        /// <param name="myConnexion">Connexion pour la base SQL</param>
        public static void AffichageStock(MySqlConnection myConnexion)
        {
            // on veut afficher le stock de chaque pièce
            ScrollingMenu scrollingMenu = new ScrollingMenu(
                "Que voulez vous afficher ?",
                0,
                Placement.TopCenter,
                new string[] { "IdPiece", "Magasin", "Fournisseur" }
            );
            Window.AddElement(scrollingMenu);
            Window.ActivateElement(scrollingMenu);
            var response = scrollingMenu.GetResponse()!.Value;
            switch (response)
            {
                case 0:
                    AfficherValue(
                        table: "__stockagepiece",
                        myConnexion,
                        printColone: "IdPiece,quantitePiece ",
                        WantColumns: new string[] { "IdPiece", "quantitePiece" }
                    );
                    break;
                case 1:
                    AfficherValue(
                        table: "__magasin",
                        myConnexion,
                        printColone: "NomMagasin,ChiffreAffairesMagasin ",
                        WantColumns: new string[] { "NomMagasin", "ChiffreAffairesMagasin" }
                    );
                    break;
                case 2:
                    AfficherValue(
                        table: "__fournisseur ",
                        myConnexion,
                        printColone: "DISTINCT __fournisseur.NomFournisseur, __stockagepiece.quantitePiece ",
                        WantColumns: new string[] { "NomFournisseur", "quantitePiece" },
                        join: @"JOIN fourni ON __fournisseur.SiretFournisseur = fourni.SiretFournisseur
                        JOIN __stockagepiece ON fourni.IdMagasin = __stockagepiece.IdMagasin; "
                    );
                    break;
            }
        }

        /// <summary>
        ///     Cette fonction permet d'ajouter les contraintes d'order by dans la requête générale d'affichage.
        /// </summary>
        /// <param name="table">La table utilisée</param>
        /// <param name="myConnexion">la connexion pour la base SQL</param>
        public static string Order(string table, MySqlConnection myConnexion)
        {
            try
            {
                string column = MenuSelector(
                    FindColonne(table, myConnexion),
                    "Choisissez une colonne pour l'ordre"
                );
                string order = MenuSelector(new string[] { "ASC", "DESC" }, "Choisissez un ordre");
                return $"ORDER BY {column} {order}";
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.Message);
                return "";
            }
        }

        /// <summary>
        /// Trouves toutes les colonnes d'une table dans la base de donnée
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="myConnexion">Connexion pour la base SQL</param>
        /// <returns></returns>
        static string[] FindColonne(string table, MySqlConnection myConnexion)
        {
            // read the data base and print every column of the table

            try
            {
                // On sélectionne toutes les colonnes de la table
                //mySqlConnection.Open();
                MySqlCommand myCommand = new MySqlCommand($"SELECT * FROM {table}", myConnexion);
                // on demande grâce au menuselector les colonnes que l'utilisateur veut afficher
                MySqlDataReader myReader = myCommand.ExecuteReader();
                string[] columns = new string[myReader.FieldCount];
                for (int i = 0; i < myReader.FieldCount; i++)
                {
                    columns[i] = myReader.GetName(i);
                }
                myReader.Close();
                //mySqlConnection.Close();
                return columns;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.Message);
                return ["erreur"];
            }
        }

        /// <summary>
        /// Trouves les tables de la base de donnée
        /// </summary>
        /// <param name="myConnexion">Connexion de la base SQL</param>
        /// <returns></returns>
        static string[] FindTable(MySqlConnection myConnexion)
        {
            try
            {
                //mySqlConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("SHOW TABLES", myConnexion);
                MySqlDataReader myReader = myCommand.ExecuteReader();

                List<string> tables = new List<string>();
                while (myReader.Read())
                {
                    tables.Add(myReader.GetString(0));
                }
                myReader.Close();
                return tables.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.Message);
                return new string[] { "erreur" };
            }
        }

        /// <summary>
        /// Cette fonction permet de modifier une valeur en accord avec la présence de clé étrangère ou non
        /// </summary>
        /// <param name="table">table utilisée</param>
        /// <param name="myConnexion">Connexion avec la base SQL</param>
        static void ModifyValue(string table, MySqlConnection myConnexion)
        {
            //select the condition of change
            string condition = Condition(table, myConnexion, modifyValue: true);
            //select the new values according to the condition
            bool foreign = false;
            if (condition == "")
            {
                Console.WriteLine(
                    "Vous n'avez pas mis de condition ou c'est une clé étrangère, vous ne pouvez pas modifier de valeur sans condition"
                );
                return;
            }
            Console.WriteLine(condition);
            Console.ReadKey();
            if (IsForeignKey(table, condition.Split(" ")[0], myConnexion))
            {
                Console.WriteLine(
                    "La colonne est une clé étrangère enfant, vous ne pouvez pas la modifier"
                );
                Console.ReadKey();
                return;
            }
            string[] listeColumn = FindColonne(table, myConnexion);
            string newValues = "";
            for (int i = 0; i < listeColumn.Length; i++)
            {
                if (listeColumn[i] != "")
                {
                    Prompt prompt = new Prompt(
                        $"Entrer la nouvelle valeur pour {listeColumn[i]}: type : {GetColumnType(table, listeColumn[i], myConnexion)} (si vous ne voulez pas mettre de nouvelle valeur pressez entré, pour mettre null entrez 'IS NULL')",
                        "",
                        Placement.TopCenter
                    );
                    Window.AddElement(prompt);
                    Window.ActivateElement(prompt);
                    var response = prompt.GetResponse()!.Value;
                    if (response != "")
                    {
                        if (
                            IsForeignKey(table, listeColumn[i], myConnexion)
                            || IsParentKey(table, listeColumn[i], myConnexion)
                        )
                        {
                            Console.WriteLine(
                                $"La colonne est une clé étrangère, vous ne pouvez pas la modifier {listeColumn[i]}"
                            );
                            Console.ReadKey();
                            foreign = true;
                        }
                        newValues += $"{listeColumn[i]} = '{response}',";
                    }
                }
            }
            if (!foreign)
            {
                if (newValues == "" || newValues == null)
                {
                    Console.WriteLine("Aucun changement n'a été souhaité");
                    return;
                }
                newValues = newValues.Substring(0, newValues.Length - 1);
                MySqlCommand myCommand = new MySqlCommand(
                    $"UPDATE {table} SET {newValues} {condition}",
                    myConnexion
                );
                myCommand.ExecuteNonQuery();
                Console.WriteLine("Valeur modifiée");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Cette fonction permet de savoir lesquelles l'utilisateur va choisir parmis celles présentes dans la base
        /// </summary>
        /// <param name="table">table utilisée dans la base de donnée</param>
        /// <param name="myConnexion">Connexion pour la base SQL</param>
        /// <param name="IsCondition">Permet de changer le message selon les cas de figures</param>
        /// <returns></returns>
        static string[] SelectColumns(
            string table,
            MySqlConnection myConnexion,
            bool IsCondition = false
        )
        {
            string message;
            if (IsCondition)
            {
                message = "Voulez vous mettre une condition de valeur sur la colonne ?";
            }
            else
            {
                message = "Voulez vous afficher la colonne ?";
            }
            try
            {
                // On récupère les colonnes que l'on souhaite de la table grâce à la fonctoin MEnuSelector
                string[] columns = FindColonne(table, myConnexion);
                string[] columnsSelected = new string[columns.Length];
                for (int i = 0; i < columns.Length; i++)
                {
                    columnsSelected[i] = MenuSelector(
                        new string[] { "Oui", "Non" },
                        $"{message} {columns[i]} ?"
                    );
                    if (columnsSelected[i] == "Non")
                    {
                        columnsSelected[i] = "";
                    }
                    else
                    {
                        columnsSelected[i] = columns[i];
                    }
                }
                return columnsSelected;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.Message);
                return new string[] { "erreur" };
            }
        }

        /// <summary>
        /// Créer un menu selector pour choisir entre plusieurs options
        /// </summary>
        /// <param name="options">les options pour l'utilisateur</param>
        /// <param name="message">le message qui sera affiché à l'utilisateur</param>
        /// <returns></returns>
        static string MenuSelector(
            string[] options,
            string message,
            Placement placement = Placement.TopCenter
        )
        {
            ScrollingMenu menu = new ScrollingMenu(message, 0, Placement.TopCenter, options);

            Window.AddElement(menu);
            Window.ActivateElement(menu);
            var response = menu.GetResponse();
            Window.RemoveElement(menu);
            return options[response!.Value];
        }

        /// <summary>
        /// Permet de faire les requêtes join entre les tables de la base de données
        /// </summary>
        /// <param name="tables">liste des tables que l'on va lier déjà présentes dans la requête </param>
        /// <param name="myConnexion">Connexion à la base SQL</param>
        /// <returns></returns>
        static string[] Join(string[] tables, MySqlConnection myConnexion)
        {
            //on choisi une des tables de table sur laquelle faire la jointure
            string table = MenuSelector(
                tables,
                "Choisissez une table qui est déjà dans la requête"
            );
            // on demande la colonne que l'on voudra utiliser pour le join
            string colonne1 = MenuSelector(
                FindColonne(table, myConnexion),
                "Choisissez une colonne que vous voudrez join"
            );
            // on demande la deuxième table
            string table2 = MenuSelector(FindTable(myConnexion), "Choisissez une table à join");
            // on demande la colonne que l'on voudra utiliser pour le join
            string colonne2 = MenuSelector(
                FindColonne(table2, myConnexion),
                "Choisissez une colonne que vous voudrez join"
            );
            colonne1 = colonne1.Trim();
            colonne2 = colonne2.Trim();
            if (
                colonne1 == colonne2
                && GetColumnType(table, colonne1, myConnexion)
                    == GetColumnType(table2, colonne2, myConnexion)
            )
            {
                // on fait la requête SQL
                if (
                    IsForeignKey(table, colonne1,  myConnexion)
                    || IsForeignKey(table2, colonne2, myConnexion)
                )
                {
                    return [table2, $"JOIN {table2} ON {table}.{colonne1} = {table2}.{colonne2}"];
                }
                else
                {
                    Console.WriteLine(
                        "Les colonnes ne sont pas compatibles pour un join car ne sont pas foreign key entre elles, Presssez une touche pour continuer"
                    );
                    Console.ReadKey();
                    return ["", ""];
                }
            }
            else
            {
                Console.WriteLine(
                    "Les colonnes ne sont pas compatibles pour un join, Presssez une touche pour continuer"
                );
                Console.ReadKey();
                return ["", ""];
            }
        }

        /// <summary>
        /// Fonction qui permet de mettre des conditions WHERE dans la requête SQL
        /// </summary>
        /// <param name="table">table utilisée pour la requête</param>
        /// <param name="myConnexion">Connexion à la base SQL</param>
        /// <param name="firstcondition">Booléan qui nous indique si c'est ou non la première condition de la requête</param>
        /// <param name="modifyValue">Booléan qui nous indique si c'est pour modifier une valeur et inclue donc les cas de foreign key</param>
        /// <returns></returns>
        static string Condition(
            string table,
            MySqlConnection myConnexion,
            bool firstcondition = true,
            bool modifyValue = false,
            bool eraseValue = false
        )
        {
            string condition = "";
            if (firstcondition)
                condition = "WHERE ";
            string colonne = MenuSelector(
                FindColonne(table, myConnexion),
                "Choisissez une colonne sur laquelle vous voulez mettre une condition"
            );
            if (modifyValue)
            {
                if (IsForeignKey(table, colonne, myConnexion))
                {
                    Console.WriteLine(
                        "La colonne est une clé étrangère, vous ne pouvez pas la modifier"
                    );
                    Console.ReadKey();
                    return "";
                }
            }
            if (eraseValue)
            {
                if (IsParentKey(table, colonne, myConnexion))
                {
                    Console.WriteLine(
                        "La colonne est une clé étrangère, vous ne pouvez pas la supprimer"
                    );
                    Console.ReadKey();
                    return "";
                }
            }
            // on récupère le type de la colonne pour permettre de proposer les bonnnes conditions
            string type = GetColumnType(table, colonne, myConnexion);
            string contrainte = "";
            switch (type)
            {
                case "int":
                    contrainte = MenuSelector(
                        new string[] { ">", "<", "=", ">=", "<=" },
                        "Choisissez une contrainte"
                    );
                    break;
                case "bool":
                    contrainte = MenuSelector(
                        new string[] { " = True", "= False" },
                        "Choisissez une contrainte"
                    );
                    break;
                case "date":
                    contrainte = MenuSelector(
                        new string[] { ">", "<", "=", ">=", "<=" },
                        "Choisissez une contrainte"
                    );
                    break;
                default:
                    contrainte = MenuSelector(
                        new string[] { "=", "IS NULL", "IS NOT NULL" },
                        "Choisissez une contrainte"
                    );
                    break;
            }

            if ($"{contrainte}" == "IS NULL" || contrainte == "IS NOT NULL")
            {
                return condition += $"{table}.{colonne} {contrainte}";
            }
            Prompt prompt = new Prompt(
                $"Entrer la condition pour : {colonne} type : {GetColumnType(table, colonne, myConnexion)}(Pressez entré si vous avez choisi IS NULL ou IS NOT NULL)",
                "",
                Placement.TopCenter
            );

            Window.AddElement(prompt);
            Window.ActivateElement(prompt);
            var response = prompt.GetResponse()!.Value;
            if (response != "")
            {
                return condition += $"{table}.{colonne} {contrainte} '{response}'";
            }
            else if (contrainte == "IS NULL")
            {
                return condition += $"{table}.{colonne} IS NULL";
            }
            else if (contrainte == "IS NOT NULL")
            {
                return condition += $"{table}.{colonne} IS NOT NULL";
            }
            else
            {
                if (type == "int")
                {
                    return condition += $"{table}.{colonne} {contrainte} 0";
                }
                else if (type == "date")
                {
                    return condition += $"{table}.{colonne} {contrainte} '0000-00-00'";
                }
                else if (type == "bool")
                {
                    return condition += $"{table}.{colonne} {contrainte} False";
                }
                else
                {
                    return condition += $"{table}.{colonne} {contrainte}";
                }
            }
        }

        /// <summary>
        /// Permet de retourner le type de la colonne pour proposer les conditions adéquates
        /// </summary>
        /// <param name="table">table de la base de donnée</param>
        /// <param name="colonne">colonne dont on veut savoir le type</param>
        /// <param name="myConnection">Connexion avec la base SQL</param>
        /// <returns></returns>
        public static string GetColumnType(
            string table,
            string colonne,
            MySqlConnection myConnection
        )
        {
            string sql =
                $"SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @table AND COLUMN_NAME = @colonne";
            using (MySqlCommand cmd = new MySqlCommand(sql, myConnection))
            {
                cmd.Parameters.AddWithValue("@table", table);
                cmd.Parameters.AddWithValue("@colonne", colonne);
                //connection.Open();
                var result = cmd.ExecuteScalar();
                //connection.Close();
                return result != null ? result.ToString() : null;
            }
        }

        /// <summary>
        /// Permet de savoir si c'est une foreign key
        /// </summary>
        /// <param name="table">table que l'on utilise dans la base de donnée</param>
        /// <param name="colonne">colonne qui est utilisée dans la table</param>
        /// <param name="myConnection">connexion avec la base SQL</param>
        /// <returns></returns>
        public static bool IsForeignKey(string table, string colonne, MySqlConnection myConnection)
        {
            // on regarde si la colonne est une clé étrangère enfant
            string sql =
                $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME = @table AND COLUMN_NAME = @colonne AND REFERENCED_TABLE_NAME IS NOT NULL";
            using (MySqlCommand cmd = new MySqlCommand(sql, myConnection))
            {
                cmd.Parameters.AddWithValue("@table", table);
                cmd.Parameters.AddWithValue("@colonne", colonne);
                //connection.Open();
                var result = cmd.ExecuteScalar();
                //connection.Close();
                return Convert.ToInt32(result) > 0;
            }
        }

        /// <summary>
        /// Permet d'effectuer une requête union entre deux tables
        /// </summary>
        /// <param name="table1"></param>
        /// <param name="table2"></param>
        /// <param name="myConnexion"></param>
        public static void Union(string table1, string table2, MySqlConnection myConnexion)
        {
            // on fait une requette union
            string colonne1 = MenuSelector(
                FindColonne(table1, myConnexion),
                "Choisissez une colonne de la première table"
            );
            string colonne2 = MenuSelector(
                FindColonne(table2, myConnexion),
                "Choisissez une colonne de la deuxième table"
            );
            MySqlCommand myCommand = new MySqlCommand(
                $"SELECT {colonne1} FROM {table1} UNION SELECT {colonne2} FROM {table2}",
                myConnexion
            );
            List<string> columns = new List<string>();
            columns.Add(colonne1);
            List<List<string>> values = new List<List<string>>();
            MySqlDataReader myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                List<string> row = new List<string>();
                row.Add(myReader[colonne1].ToString());
                values.Add(row);
            }
            myReader.Close();
            TableView tableView = new TableView($"Values de {table1}", columns, values);
            Window.AddElement(tableView);
            Window.ActivateElement(tableView);
        }

        /// <summary>
        /// On vérifie si la colonne est une reference
        /// </summary>
        /// <param name="table1">table dont est issue la colonne</param>
        /// <param name="colonne">colonne dont on cherche si oui ou non c'est une référence</param>
        /// <param name="myConnexion">connexion avec la base SQL</param>
        /// <returns></returns>
        public static bool IsParentKey(string table1, string colonne, MySqlConnection myConnexion)
        {
            string query =
                @"
            SELECT COUNT(*)
            FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
            WHERE REFERENCED_TABLE_NAME = @table AND REFERENCED_COLUMN_NAME = @colonne";

            using (MySqlCommand cmd = new MySqlCommand(query, myConnexion))
            {
                cmd.Parameters.AddWithValue("@table", table1);
                cmd.Parameters.AddWithValue("@colonne", colonne);

                var result = cmd.ExecuteScalar();
                return Convert.ToInt32(result) > 0;
            }
        }

        /// <summary>
        /// Permet de savoir si une colonne d'une table est bien étrangère à l'autre
        /// </summary>
        /// <param name="table">table que l'on utilise </param>
        /// <param name="colonne">colonne qui est foreign key</param>
        /// <param name="refTable">table de référence</param>
        /// <param name="refColonne">colonne dont on essaye de voir si c'est la refereence</param>
        /// <param name="myConnection"></param>
        /// <returns></returns>
        public static bool IsChildKey(
            string table,
            string colonne,
            string refTable,
            string refColonne,
            MySqlConnection myConnection
        )
        {
            string sql =
                @"
                SELECT COUNT(*)
                FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                WHERE TABLE_NAME = @table
                AND COLUMN_NAME = @colonne
                AND REFERENCED_TABLE_NAME = @refTable
                AND REFERENCED_COLUMN_NAME = @refColonne
            ";
            using (MySqlCommand command = new MySqlCommand(sql, myConnection))
            {
                command.Parameters.AddWithValue("@table", table);
                command.Parameters.AddWithValue("@colonne", colonne);
                command.Parameters.AddWithValue("@refTable", refTable);
                command.Parameters.AddWithValue("@refColonne", refColonne);

                int count = Convert.ToInt32(command.ExecuteScalar());

                return count > 0;
            }
        }


        /// <summary>
        /// On essaye de supprier une valeur dans une table
        /// </summary>
        /// <param name="table">table dans laquelle on veut supprimer une valeur</param>
        /// <param name="myConnexion">connexion avec la base sql</param>
        public static void SupprimerValue(string table, MySqlConnection myConnexion)
        {
            try
            {
                string condition = Condition(table, myConnexion, eraseValue: true);
                Console.ReadKey();
                if (condition == "")
                {
                    Console.WriteLine(
                        "Vous n'avez pas mis de condition ou c'est une clé étrangère, vous ne pouvez pas supprimer de valeur sans condition"
                    );
                    Console.ReadKey();
                    return;
                }
                else if (IsParentKey(table, condition.Split(" ")[0], myConnexion))
                {
                    Console.WriteLine(
                        "La colonne est une clé étrangère parent, vous ne pouvez pas la supprimer"
                    );
                    Console.ReadKey();
                    return;
                }
                MySqlCommand myCommand = new MySqlCommand(
                    $"DELETE FROM {table} {condition}",
                    myConnexion
                );
                Console.ReadKey();
                myCommand.ExecuteNonQuery();
                Console.WriteLine("Valeur supprimée");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.Message);
                Console.ReadKey();
            }
        }
    }
}