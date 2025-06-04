-- Active: 1713775000160@@127.0.0.1@3306@velomax
#------------------------------------------------------------
#        Script MySQL.
#------------------------------------------------------------


#------------------------------------------------------------
# Table: __magasin
#------------------------------------------------------------
CREATE TABLE __magasin(
        IdMagasin              Int  Auto_increment  NOT NULL ,
        NomMagasin             Varchar (50) NOT NULL ,
        AdresseMagasin         Varchar (255) NOT NULL ,
        idGerant               Int NOT NULL ,
        ChiffreAffairesMagasin Double NOT NULL ,
        SatistificationClient  Int NOT NULL
	,CONSTRAINT __magasin_PK PRIMARY KEY (IdMagasin)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: __Vendeur
#------------------------------------------------------------

CREATE TABLE __Vendeur(
        IdVendeur     Int  Auto_increment  NOT NULL ,
        NomVendeur    Varchar (11) NOT NULL ,
        StatutVendeur Varchar (25) NOT NULL ,
        IdMagasin     Int NOT NULL
	,CONSTRAINT __Vendeur_PK PRIMARY KEY (IdVendeur)
	,CONSTRAINT _Vendeur__magasin_FK FOREIGN KEY (IdMagasin) REFERENCES __magasin(IdMagasin)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: __Modèle de Bycyclette
#------------------------------------------------------------

CREATE TABLE __Modele_de_Bycyclette(
        IdProduit                  Int Auto_increment NOT NULL ,
        NomProduit                 Varchar (255) NOT NULL ,
        GrandeurProduit            Varchar (255) NOT NULL ,
        PrixProduit                Decimal (10) NOT NULL ,
        DateIntroductionProduit    Date NOT NULL ,
        DateDiscontinuationProduit Date NOT NULL ,
        LigneProduit               Varchar (2) NOT NULL
	,CONSTRAINT __Modele_de_Bycyclette_PK PRIMARY KEY (IdProduit)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: __Fournisseur
#------------------------------------------------------------

CREATE TABLE __Fournisseur(
        SiretFournisseur      Int  Auto_increment  NOT NULL ,
        NomFournisseur        Varchar (25) NOT NULL ,
        ContactFournisseur    Varchar (25) NOT NULL ,
        AdresseFournisseur    Varchar (255) NOT NULL ,
        ReactiviteFournisseur Int NOT NULL
	,CONSTRAINT __Fournisseur_PK PRIMARY KEY (SiretFournisseur)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: __Pièce
#------------------------------------------------------------

CREATE TABLE __Piece(
        IdPiece                      Int  Auto_increment  NOT NULL ,
        DescriptionPiece             Varchar (255) NOT NULL ,
        NumPieceCatalogueFournisseur Int NOT NULL ,
        PrixPiece                    Decimal (10,2) NOT NULL ,
        DateIntroductionPiece        Date NOT NULL ,
        DateDiscontinuationPiece     Date NOT NULL ,
        DelaiApprovisionnementPiece  Int NOT NULL ,
        SiretFournisseur             Int NOT NULL
	,CONSTRAINT __Piece_PK PRIMARY KEY (IdPiece)
	,CONSTRAINT _Piece__Fournisseur_FK FOREIGN KEY (SiretFournisseur) REFERENCES __Fournisseur(SiretFournisseur)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: ___ClientParticulier
#------------------------------------------------------------

CREATE TABLE ___ClientParticulier(
        IdClientParticulier        Int  Auto_increment  NOT NULL ,
        NomClientParticulier       Varchar (255) NOT NULL ,
        PrenomClientParticulier    Varchar (255) NOT NULL ,
        AdresseClientParticulier   Varchar (255) NOT NULL ,
        TelephoneClientParticulier Varchar (20) NOT NULL ,
        CourrielClientParticulier  Varchar (255) NOT NULL ,
        TypeClient                 Varchar (50) NOT NULL
	,CONSTRAINT ___ClientParticulier_PK PRIMARY KEY (IdClientParticulier)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: __Programme
#------------------------------------------------------------

CREATE TABLE __Programme(
        IdProgramme          Int  Auto_increment  NOT NULL ,
        DescriptionProgramme Varchar (255) NOT NULL ,
        CoutPROGRAMME        Decimal (10,2) NOT NULL ,
        RabaisROGRAMME       Int NOT NULL ,
        DureeProgramme       Varchar (255) NOT NULL
	,CONSTRAINT __Programme_PK PRIMARY KEY (IdProgramme)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: ____ClientPro
#------------------------------------------------------------

CREATE TABLE ____ClientPro(
        IdClientPro               Int  Auto_increment  NOT NULL ,
        NomClientPro              Varchar (255) NOT NULL ,
        ContactClientPro          Varchar (255) NOT NULL ,
        AdresseClientPro          Varchar (255) NOT NULL ,
        TelephoneClientPro        Varchar (20) NOT NULL ,
        CourrielClientParticulier Varchar (255) NOT NULL ,
        TypeClienPro              Varchar (50) NOT NULL ,
        TauxRemise                Decimal NOT NULL
	,CONSTRAINT ____ClientPro_AK UNIQUE (TauxRemise)
	,CONSTRAINT ____ClientPro_PK PRIMARY KEY (IdClientPro)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: __CommandePro
#------------------------------------------------------------

CREATE TABLE __CommandePro(
        IdCommandePRO               Int  Auto_increment  NOT NULL ,
        DateCommandePRO             Date NOT NULL ,
        AdresseLivraisonCommandePRO Varchar (25) NOT NULL ,
        DateLivraisonCommandePRO    Date NOT NULL ,
        QualiteCommandePRO          Varchar (25) NOT NULL ,
        IdClientPro                 Int NOT NULL ,
        IdVendeur                   Int NOT NULL
	,CONSTRAINT __CommandePro_PK PRIMARY KEY (IdCommandePRO)
	,CONSTRAINT _CommandePro__ClientPro_FK FOREIGN KEY (IdClientPro) REFERENCES ____ClientPro(IdClientPro)
	,CONSTRAINT _CommandePro__Vendeur0_FK FOREIGN KEY (IdVendeur) REFERENCES __Vendeur(IdVendeur)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: ___Commande_PARTICULIER
#------------------------------------------------------------

CREATE TABLE ___Commande_PARTICULIER(
        IdCommandePARTICULIER               Int  Auto_increment  NOT NULL ,
        DateCommandePARTICULIER             Date NOT NULL ,
        AdresseLivraisonCommandePARTICULIER Varchar (25) NOT NULL ,
        DateLivraisonCommandePARTICULIER    Date NOT NULL ,
        QualiteCommandePARTICULIER          Varchar (25) NOT NULL ,
        IdClientParticulier                 Int NOT NULL ,
        IdVendeur                           Int NOT NULL
	,CONSTRAINT ___Commande_PARTICULIER_PK PRIMARY KEY (IdCommandePARTICULIER)
	,CONSTRAINT __Commande_PARTICULIER_ClientParticulier_FK FOREIGN KEY (IdClientParticulier) REFERENCES ___ClientParticulier(IdClientParticulier)
	,CONSTRAINT __Commande_PARTICULIER__Vendeur0_FK FOREIGN KEY (IdVendeur) REFERENCES __Vendeur(IdVendeur)
)ENGINE=InnoDB;




#------------------------------------------------------------
# Table: __comprendParticulierPièce
#------------------------------------------------------------

CREATE TABLE __comprendParticulierPiece(
        IdPiece       Int NOT NULL ,
        IdCommandePRO Int NOT NULL
	,CONSTRAINT __comprendParticulierPiece_PK PRIMARY KEY (IdPiece,IdCommandePRO)
	,CONSTRAINT _comprendParticulierPiece__Piece_FK FOREIGN KEY (IdPiece) REFERENCES __Piece(IdPiece)
	,CONSTRAINT _comprendParticulierPiece__CommandePro0_FK FOREIGN KEY (IdCommandePRO) REFERENCES __CommandePro(IdCommandePRO)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: __stockage
#------------------------------------------------------------

CREATE TABLE __stockage(
        IdPiece       Int NOT NULL ,
        IdMagasin     Int NOT NULL ,
        quantitePiece Int NOT NULL
	,CONSTRAINT __stockage_PK PRIMARY KEY (IdPiece,IdMagasin)
	,CONSTRAINT _stockage__Piece_FK FOREIGN KEY (IdPiece) REFERENCES __Piece(IdPiece)
	,CONSTRAINT _stockage__magasin0_FK FOREIGN KEY (IdMagasin) REFERENCES __magasin(IdMagasin)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: __comprendParticulierVélo
#------------------------------------------------------------

CREATE TABLE __comprendParticulierVelo(
        IdProduit     Int NOT NULL ,
        IdCommandePRO Int NOT NULL
	,CONSTRAINT __comprendParticulierVelo_PK PRIMARY KEY (IdProduit,IdCommandePRO)
	,CONSTRAINT _comprendParticulierVelo__Modele_de_Bycyclette_FK FOREIGN KEY (IdProduit) REFERENCES __Modele_de_Bycyclette(IdProduit)
	,CONSTRAINT _comprendParticulierVelo__CommandePro0_FK FOREIGN KEY (IdCommandePRO) REFERENCES __CommandePro(IdCommandePRO)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: __stockageVélo
#------------------------------------------------------------

CREATE TABLE __stockageVelo(
        IdMagasin    Int NOT NULL ,
        IdProduit    Int NOT NULL ,
        quantiteVelo Int NOT NULL
	,CONSTRAINT __stockageVelo_PK PRIMARY KEY (IdMagasin,IdProduit)
	,CONSTRAINT _stockageVelo__magasin_FK FOREIGN KEY (IdMagasin) REFERENCES __magasin(IdMagasin)
	,CONSTRAINT _stockageVelo__Modele_de_Bycyclette0_FK FOREIGN KEY (IdProduit) REFERENCES __Modele_de_Bycyclette(IdProduit)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: __assemble
#------------------------------------------------------------

CREATE TABLE __assemble(
        IdProduit Int NOT NULL ,
        IdPiece   Int NOT NULL
	,CONSTRAINT __assemble_PK PRIMARY KEY (IdProduit,IdPiece)
	,CONSTRAINT _assemble__Modele_de_Bycyclette_FK FOREIGN KEY (IdProduit) REFERENCES __Modele_de_Bycyclette(IdProduit)
	,CONSTRAINT _assemble__Piece0_FK FOREIGN KEY (IdPiece) REFERENCES __Piece(IdPiece)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: ___comprendVélo
#------------------------------------------------------------

CREATE TABLE ___comprendVelo(
        IdCommandePARTICULIER Int NOT NULL ,
        IdProduit             Int NOT NULL
	,CONSTRAINT ___comprendVelo_PK PRIMARY KEY (IdCommandePARTICULIER,IdProduit)
	,CONSTRAINT __comprendVelo_Commande_PARTICULIER_FK FOREIGN KEY (IdCommandePARTICULIER) REFERENCES ___Commande_PARTICULIER(IdCommandePARTICULIER)
	,CONSTRAINT __comprendVelo__Modele_de_Bycyclette0_FK FOREIGN KEY (IdProduit) REFERENCES __Modele_de_Bycyclette(IdProduit)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: ___comprendProPièce
#------------------------------------------------------------

CREATE TABLE ___comprendProPiece(
        IdCommandePARTICULIER Int NOT NULL ,
        IdPiece               Int NOT NULL
	,CONSTRAINT ___comprendProPiece_PK PRIMARY KEY (IdCommandePARTICULIER,IdPiece)
	,CONSTRAINT __comprendProPiece_Commande_PARTICULIER_FK FOREIGN KEY (IdCommandePARTICULIER) REFERENCES ___Commande_PARTICULIER(IdCommandePARTICULIER)
	,CONSTRAINT __comprendProPiece__Piece0_FK FOREIGN KEY (IdPiece) REFERENCES __Piece(IdPiece)
)ENGINE=InnoDB;
-- Table: Fourni
CREATE TABLE Fourni (
    IdMagasin INT NOT NULL,
    SiretFournisseur INT NOT NULL,
    PRIMARY KEY (IdMagasin, SiretFournisseur),
    FOREIGN KEY (IdMagasin) REFERENCES __magasin(IdMagasin),
    FOREIGN KEY (SiretFournisseur) REFERENCES __Fournisseur(SiretFournisseur)
) ENGINE=InnoDB;
-- Table: CommanderParticulier
CREATE TABLE CommanderParticulier (
    IdClientParticulier INT NOT NULL,
    IdCommandePARTICULIER INT NOT NULL,
    PRIMARY KEY (IdClientParticulier, IdCommandePARTICULIER),
    FOREIGN KEY (IdClientParticulier) REFERENCES ___ClientParticulier(IdClientParticulier),
    FOREIGN KEY (IdCommandePARTICULIER) REFERENCES ___Commande_PARTICULIER(IdCommandePARTICULIER)
) ENGINE=InnoDB;
-- Table: adhesion
CREATE TABLE adhesion (
    IdClientParticulier INT NOT NULL,
    IdProgramme INT NOT NULL,
    DateDebutAdhesion DATE NOT NULL,
    DateFinAdhesion DATE NOT NULL,
    PRIMARY KEY (IdClientParticulier, IdProgramme),
    FOREIGN KEY (IdClientParticulier) REFERENCES ___ClientParticulier(IdClientParticulier),
    FOREIGN KEY (IdProgramme) REFERENCES __Programme(IdProgramme)
) ENGINE=InnoDB;/* 2024-04-25 11:03:51 [17 ms] */ 


/* 2024-04-25 11:04:04 [52 ms] */ 




#les insert qui marche 
ALTER TABLE __Piece MODIFY COLUMN NumPieceCatalogueFournisseur VARCHAR(50);

INSERT INTO __Fournisseur (NomFournisseur, ContactFournisseur, AdresseFournisseur, ReactiviteFournisseur, SiretFournisseur) 
VALUES ('FournisseurABC', 'John Doe', '123 Rue des Fournisseurs', 5, 123456789);

INSERT INTO __Piece (DescriptionPiece, IdPiece, PrixPiece, DateIntroductionPiece, DateDiscontinuationPiece, DelaiApprovisionnementPiece, NumPieceCatalogueFournisseur, SiretFournisseur)
VALUES 
('Cadre', 1, 100.00, '2024-01-01', '2024-12-31', 7, 'C26', 123456789),
('Guidon', 2, 50.00, '2024-01-01', '2024-12-31', 7, 'G7', 123456789),
('Freins', 3, 30.00, '2024-01-01', '2024-12-31', 7, 'F3', 123456789),
('Selle', 4, 20.00, '2024-01-01', '2024-12-31', 7, 'S87', 123456789),
('DérailleurAvant', 5, 80.00, '2024-01-01', '2024-12-31', 7, 'DV133', 123456789),
('DérailleurArrière', 6, 70.00, '2024-01-01', '2024-12-31', 7, 'DR52', 123456789),
('RoueAvant', 7, 60.00, '2024-01-01', '2024-12-31', 7, 'R44', 123456789),
('RoueArrière', 8, 60.00, '2024-01-01', '2024-12-31', 7, 'R47', 123456789),
('Réflecteurs', 9, 5.00, '2024-01-01', '2024-12-31', 7, NULL, 123456789),
('Pédalier', 10, 25.00, '2024-01-01', '2024-12-31', 7, 'P12', 123456789),
('Ordinateur', 11, 15.00, '2024-01-01', '2024-12-31', 7, NULL, 123456789),
('Panier', 12, 10.00, '2024-01-01', '2024-12-31', 7, NULL, 123456789);

INSERT INTO __Programme (DescriptionProgramme, CoutPROGRAMME, DureeProgramme, RabaisROGRAMME)
VALUES 
('Fidélio', 15.00, '1 an', 5),
('Fidélio Or', 25.00, '2 ans', 8),
('Fidélio Platine', 60.00, '2 ans', 10),
('Fidélio Max', 100.00, '3 ans', 12);

ALTER TABLE __Modele_de_Bycyclette
MODIFY COLUMN LigneProduit VARCHAR(20);
  
INSERT INTO __Modele_de_Bycyclette (NomProduit, GrandeurProduit, PrixProduit, DateIntroductionProduit, DateDiscontinuationProduit, LigneProduit)
VALUES 
    ('Kilimandjaro', 'Adultes', 569.00, '2023-03-15', '2025-06-30', 'VTT'),
    ('NorthPole', 'Adultes', 329.00, '2023-04-01', '2025-08-31', 'VTT'),
    ('MontBlanc', 'Jeunes', 399.00, '2023-02-15', '2025-07-15', 'VTT'),
    ('Hooligan', 'Jeunes', 199.00, '2023-01-20', '2025-06-01', 'VTT'),
    ('Orléans', 'Hommes', 229.00, '2023-03-01', '2025-09-30', 'Vélo de course'),
    ('Orléans', 'Dames', 229.00, '2023-03-01', '2025-09-30', 'Vélo de course'),
    ('BlueJay', 'Hommes', 349.00, '2023-02-01', '2025-08-31', 'Vélo de course'),
    ('BlueJay', 'Dames', 349.00, '2023-02-01', '2025-08-31', 'Vélo de course'),
    ('Trail Explorer', 'Filles', 129.00, '2023-01-15', '2025-07-15', 'Classique'),
    ('Trail Explorer', 'Garçons', 129.00, '2023-01-15', '2025-07-15', 'Classique'),
    ('Night Hawk', 'Jeunes', 189.00, '2023-02-15', '2025-08-15', 'Classique'),
    ('Tierra Verde', 'Hommes', 199.00, '2023-03-01', '2025-09-30', 'Classique'),
    ('Tierra Verde', 'Dames', 199.00, '2023-03-01', '2025-09-30', 'Classique'),
    ('Mud Zinger I', 'Jeunes', 279.00, '2023-01-15', '2025-07-15', 'BMX'),
    ('Mud Zinger II', 'Adultes', 359.00, '2023-04-01', '2025-08-31', 'BMX');

INSERT INTO __Fournisseur (NomFournisseur, ContactFournisseur, AdresseFournisseur, ReactiviteFournisseur, SiretFournisseur)
VALUES ('FournisseurXYZ', 'Jane Smith', '456 Avenue des Approvisionnements', 4, 987654321),
       ('Fournisseur123', 'Bob Johnson', '789 Rue des Livraisons', 3, 246810975),
       ('Fournisseur456', 'Alice Brown', '321 Boulevard des Stocks', 4, 135792468);

INSERT INTO __Vendeur (NomVendeur, StatutVendeur, IdMagasin) 
VALUES 
    ('Vendeur 1', 'Temps plein', 1),
    ('Vendeur 2', 'Temps partiel', 2),
    ('Vendeur 3', 'Temps plein', 3);


INSERT INTO __magasin (NomMagasin, AdresseMagasin, idGerant, ChiffreAffairesMagasin, SatistificationClient) 
VALUES 
    ('Magasin A', 'Adresse 1', 1, 100000, 90),
    ('Magasin B', 'Adresse 2', 2, 150000, 85),
    ('Magasin C', 'Adresse 3', 3, 120000, 88);
INSERT INTO ___ClientParticulier (NomClientParticulier, PrenomClientParticulier, AdresseClientParticulier, TelephoneClientParticulier, CourrielClientParticulier, TypeClient) 
VALUES 
    ('ClientParticulier 1', 'Prenom 1', 'Adresse 1', '1234567890', 'client1@example.com', 'Type 1'),
    ('ClientParticulier 2', 'Prenom 2', 'Adresse 2', '1234567891', 'client2@example.com', 'Type 2'),
    ('ClientParticulier 3', 'Prenom 3', 'Adresse 3', '1234567892', 'client3@example.com', 'Type 3');

INSERT INTO __magasin (NomMagasin, AdresseMagasin, idGerant, ChiffreAffairesMagasin, SatistificationClient)
VALUES ('Magasin A', '123 Rue des Magasins', 1, 100000.00, 85),
       ('Magasin B', '456 Avenue des Commerces', 2, 75000.00, 90),
       ('Magasin C', '789 Boulevard des Boutiques', 3, 120000.00, 88);

-- Insertions pour la table __stockage
INSERT INTO __stockage (IdPiece, IdMagasin, quantitePiece)
VALUES 
    (1, 1, 50),
    (2, 2, 100);

-- Insertions pour la table __stockageVélo
INSERT INTO __stockageVelo (IdMagasin, IdProduit, quantiteVelo)
VALUES 
    (1, 1, 20),
    (2, 2, 30);

-- Insertions pour la table __assemble
INSERT INTO __assemble (IdProduit, IdPiece)
VALUES 
    (1, 1),
    (2, 2);
-- Insertions pour la table Fourni
INSERT INTO Fourni (IdMagasin, SiretFournisseur)
VALUES 
    (1, 123456789),
    (2, 987654321);
-- Insertions pour la table adhesion
INSERT INTO adhesion (IdClientParticulier, IdProgramme, DateDebutAdhesion, DateFinAdhesion)
VALUES 
    (1, 1, '2023-01-01', '2024-01-01'),
    (2, 2, '2023-02-01', '2024-02-01');
INSERT INTO __Modele_de_Bycyclette (NomProduit, GrandeurProduit, PrixProduit, DateIntroductionProduit, DateDiscontinuationProduit, LigneProduit)
VALUES 
    ('Kilimandjaro', 'Adultes', 569.00, '2023-01-01', '2024-12-31', 'VTT'),
    ('NorthPole', 'Adultes', 329.00, '2022-06-15', '2024-06-14', 'VTT'),
    ('MontBlanc', 'Jeunes', 399.00, '2022-03-20', '2024-03-19', 'VTT');

-- Correction des insertions pour la table __Piece
INSERT INTO __Piece (DescriptionPiece, NumPieceCatalogueFournisseur, PrixPiece, DateIntroductionPiece, DateDiscontinuationPiece, DelaiApprovisionnementPiece, SiretFournisseur)
VALUES 
    ('Roue de vélo', 'R123', 35.00, '2023-01-01', '2024-12-31', 7, 123456789),
    ('Pédale de vélo', 'P456', 15.00, '2022-06-15', '2024-06-14', 5, 987654321);
ALTER TABLE __Vendeur
MODIFY COLUMN NomVendeur VARCHAR(50); -- Modifier la taille en fonction de vos besoins
-- Correction des insertions pour la table __Vendeur
INSERT INTO __Vendeur (NomVendeur, StatutVendeur, IdMagasin)
VALUES 
    ('Jean Dupont', 'Temps plein', 1),
    ('Alic Martin', 'Temps partiel', 1),
    ('Paul D.', 'Temps plein', 2);

INSERT INTO __Fournisseur (NomFournisseur, ContactFournisseur, AdresseFournisseur, ReactiviteFournisseur, SiretFournisseur)
VALUES 
    ('FournisseurABC', 'John Doe', '123 Rue des Fournisseurs', 5, 7572579),
    ('FournisseurXYZ', 'Jane Smith', '456 Avenue des Fournisseurs', 4, 61549846);
ALTER TABLE __CommandePro MODIFY COLUMN AdresseLivraisonCommandePRO VARCHAR(255);
INSERT INTO __CommandePro (DateCommandePRO, AdresseLivraisonCommandePRO, DateLivraisonCommandePRO, QualiteCommandePRO, IdClientPro, IdVendeur)
VALUES 
    ('2023-01-01', '123 Rue de la République', '2023-01-05', 'Bonne', 1, 1),
    ('2023-02-01', '456 Avenue des Champs-Élysées', '2023-02-05', 'Bonne', 2, 2);
ALTER TABLE ___Commande_PARTICULIER MODIFY COLUMN AdresseLivraisonCommandePARTICULIER VARCHAR(255);

-- Correction des insertions pour la table ___Commande_PARTICULIER
INSERT INTO ___Commande_PARTICULIER (DateCommandePARTICULIER, AdresseLivraisonCommandePARTICULIER, DateLivraisonCommandePARTICULIER, QualiteCommandePARTICULIER, IdClientParticulier, IdVendeur)
VALUES 
    ('2023-01-01', '123 Rue de la République', '2023-01-05', 'Bonne', 1, 1),
    ('2023-02-01', '456 Avenue des Champs-Élysées', '2023-02-05', 'Bonne', 2, 2);
INSERT INTO __magasin (NomMagasin, AdresseMagasin, idGerant, ChiffreAffairesMagasin, SatistificationClient)
VALUES 
    ('Magasin A', '123 Rue de la République', 1, 10000.00, 85),
    ('Magasin B', '456 Avenue des Champs-Élysées', 2, 15000.00, 90),
    ('Magasin C', '789 Boulevard Haussmann', 3, 20000.00, 95);

INSERT INTO __Modele_de_Bycyclette (NomProduit, GrandeurProduit, PrixProduit, DateIntroductionProduit, DateDiscontinuationProduit, LigneProduit)
VALUES 
    ('Kilimandjaro', 'Adultes', 569.00, '2023-01-01', '2024-12-31', 'VTT'),
    ('NorthPole', 'Adultes', 329.00, '2022-06-15', '2024-06-14', 'VTT'),
    ('MontBlanc', 'Jeunes', 399.00, '2022-03-20', '2024-03-19', 'VTT');

INSERT INTO __Piece (DescriptionPiece, NumPieceCatalogueFournisseur, PrixPiece, DateIntroductionPiece, DateDiscontinuationPiece, DelaiApprovisionnementPiece, SiretFournisseur)
VALUES 
    ('Roue de vélo', 'R123', 35.00, '2023-01-01', '2024-12-31', 7, 123456789),
    ('Pédale de vélo', 'P456', 15.00, '2022-06-15', '2024-06-14', 5, 987654321);

INSERT INTO ___ClientParticulier (NomClientParticulier, PrenomClientParticulier, AdresseClientParticulier, TelephoneClientParticulier, CourrielClientParticulier, TypeClient)
VALUES 
    ('Dupont', 'Jean', '123 Rue de la République', '0123456789', 'jean.dupont@example.com', 'Particulier'),
    ('Martin', 'Alice', '456 Avenue des Champs-Élysées', '0987654321', 'alice.martin@example.com', 'Particulier');
INSERT INTO __Programme (DescriptionProgramme, CoutPROGRAMME, RabaisROGRAMME, DureeProgramme)
VALUES 
    ('Fidélio', 15.00, 5, '1 an'),
    ('Fidélio Or', 25.00, 8, '2 ans'),
    ('Fidélio Platine', 60.00,10,'2 ans');


CREATE USER IF NOT EXISTS 'bozo'@'localhost' IDENTIFIED BY 'bozo';
GRANT ALL PRIVILEGES ON velomax.* TO 'bozo'@'localhost';
FLUSH PRIVILEGES;