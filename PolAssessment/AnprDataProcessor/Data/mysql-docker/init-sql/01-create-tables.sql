
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
