
CREATE TABLE AnprRecords (
    Id BIGINT AUTO_INCREMENT PRIMARY KEY,

    LicensePlate VARCHAR(20) NOT NULL,
    Longitude DOUBLE NOT NULL,
    Latitude DOUBLE NOT NULL,
    ExactDateTime DATETIME NOT NULL,

	VehicleTechnicalName VARCHAR(255) NULL,
    VehicleBrandName VARCHAR(255) NULL,
    VehicleApkExpirationDate DATETIME NULL,

	LocationStreet VARCHAR(255) NULL,
    LocationCity VARCHAR(255) NULL
);

CREATE TABLE UploadUsers (
    Id INT PRIMARY KEY,
    UserDescription VARCHAR(255) NOT NULL,
    ClientId VARCHAR(64) NOT NULL,
    ClientSecret VARCHAR(64) NOT NULL
);

CREATE TABLE AnprRecordUploadUsers (
    AnprRecordId BIGINT,
    UploadUserId INT,
    ExactDateTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (AnprRecordId, UploadUserId),
    FOREIGN KEY (AnprRecordId) REFERENCES AnprRecords(Id) ON DELETE CASCADE,
    FOREIGN KEY (UploadUserId) REFERENCES UploadUsers(Id) ON DELETE CASCADE
);

-- Tables for ASP.NET Core Identity
CREATE DATABASE IF NOT EXISTS webapi;

USE webapi;

CREATE TABLE `AspNetRoles` (
  `Id` varchar(256) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `AspNetUsers` (
  `Id` varchar(256) NOT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `ConcurrencyStamp` longtext,
  `PhoneNumber` longtext,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `EmailIndex` (`NormalizedEmail`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `AspNetUserClaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(256) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `AspNetUserLogins` (
  `LoginProvider` varchar(256) NOT NULL,
  `ProviderKey` varchar(256) NOT NULL,
  `ProviderDisplayName` longtext,
  `UserId` varchar(256) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `AspNetUserRoles` (
  `UserId` varchar(256) NOT NULL,
  `RoleId` varchar(256) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `AspNetUserTokens` (
  `UserId` varchar(256) NOT NULL,
  `LoginProvider` varchar(256) NOT NULL,
  `Name` varchar(256) NOT NULL,
  `Value` longtext,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `AspNetRoleClaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(256) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
