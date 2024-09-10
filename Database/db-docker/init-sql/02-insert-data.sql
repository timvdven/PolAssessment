USE anprdata;
INSERT INTO UploadUsers (Id, UserDescription, ClientId, HashedClientSecret)
VALUES
(1, 'ANPR Enricher System', '3d6f0d98f00b204e9800998ecf8427e', '101a57f8d7efd05310997baf73fdbbf55a17b4fbd00136fa6b92e6af6e82937a');

-- Optionally insert some test data
-- INSERT INTO `AnprRecords` (`LicensePlate`, `Longitude`, `Latitude`, `ExactDateTime`, `VehicleTechnicalName`, `VehicleBrandName`, `VehicleApkExpirationDate`, `LocationStreet`, `LocationCity`) VALUES
-- ('95-JPP-1', 5.21054989, 51.60679245, '2024-08-27 07:15:17', 'TOYOTA AYGO', 'TOYOTA', '2025-06-26 00:00:00', 'Mgr Zwijsenstraat', 'Haaren'),
-- ('95-JKH-9', 5.64461345, 51.43463573, '2024-08-29 14:38:06', 'TOYOTA YARIS', 'TOYOTA', '2025-06-26 00:00:00', 'Broekkant', 'Lierop'),
-- ('95-JSX-3', 5.62235749, 52.15305166, '2024-08-25 22:55:05', 'SAAB 900', 'SAAB', '2023-10-06 00:00:00', 'Hoornweg', 'Barneveld'),
-- ('95-JSG-9', 4.61528181, 51.58948914, '2024-08-30 23:43:24', 'SUMMER EDITION 011', 'DETHLEFFS', '2024-05-30 00:00:00', 'Bankenstraat', 'Etten-Leur'),
-- ('95-JRB-4', 4.67526953, 51.93423947, '2024-08-29 11:10:24', 'AUDI A6', 'AUDI', '2025-07-23 00:00:00', 'Oudelandseweg', 'Ouderkerk aan den IJssel'),
-- ('95-JR-FV', 5.78577062, 51.91856815, '2024-08-26 16:54:24', '5ER REIHE', 'BMW', '2020-01-03 00:00:00', 'Parallelweg-Noord', 'Valburg'),
-- ('95-JNK-5', 5.39820446, 51.62169338, '2024-08-26 02:58:35', 'TOYOTA AYGO', 'TOYOTA', '2025-07-21 00:00:00', 'Oetelaar', 'Schijndel'),
-- ('95-JPS-7', 6.28391634, 52.06818048, '2024-08-28 10:48:53', 'A4 S-LINE', 'AUDI', '2024-09-16 00:00:00', 'Wichmondseweg', 'Hengelo (Gld)'),
-- ('95-JLZ-6', 4.80476868, 52.02604555, '2024-08-26 00:14:45', 'ASTRA CABRIO', 'OPEL', '2025-05-22 00:00:00', 'Hogebrug', 'Driebruggen'),
-- ('13-HHP-2', 5.51033819, 51.88631924, '2024-08-29 04:45:03', 'FOX', 'VOLKSWAGEN', '2025-08-01 00:00:00', 'Ringkade', 'Beneden-Leeuwen'),
-- ('99-GFG-8', 6.04972314, 52.03449337, '2024-08-26 00:03:37', 'TIGUAN', 'VOLKSWAGEN', '2025-07-05 00:00:00', 'Diepesteeg', 'De Steeg'),
-- ('95-JPH-2', 5.00637021, 52.42590187, '2024-08-30 03:49:09', 'INSIGNIA', 'OPEL', '2023-03-23 00:00:00', 'Belmermeer', 'Broek in Waterland'),
-- ('95-JLS-4', 5.22766628, 51.20822852, '2024-08-27 18:36:38', 'E 220 CDI', 'MERCEDES-BENZ', '2025-04-28 00:00:00', 'Wolstraat', 'Wezel'),
-- ('95-JP-RD', 4.15979073, 51.74732788, '2024-08-25 00:24:15', 'AGILA', 'OPEL', '2024-11-13 00:00:00', 'Langeweg', 'Sommelsdijk'),
-- ('GS-921-T', 4.55529418, 52.46798994, '2024-08-29 05:17:16', '308', 'PEUGEOT', '2025-06-01 00:00:00', 'Noordpier', 'Velsen-Noord'),
-- ('95-JR-FF', 5.26051506, 51.31091955, '2024-08-28 18:53:43', 'YARIS', 'TOYOTA', '2024-08-28 00:00:00', 'Witrijt', 'Bergeijk'),
-- ('95-JP-VD', 6.42519372, 51.73034342, '2024-08-30 00:55:32', 'KANGOO', 'RENAULT', '2025-07-01 00:00:00', 'Husenweg', 'Xanten'),
-- ('95-JS-VT', 6.40386241, 51.69299579, '2024-08-26 16:25:46', 'FABIA', 'SKODA', '2025-06-02 00:00:00', 'Kreuzstraße', 'Xanten'),
-- ('95-JKS-3', 5.17766285, 52.36634808, '2024-08-29 20:20:41', 'TOYOTA PRIUS', 'TOYOTA', '2025-05-10 00:00:00', 'Elvis Presleystraat', 'Almere'),
-- ('95-JKJ-9', 5.91409304, 51.3738061, '2024-08-27 23:41:18', 'C4 PICASSO', 'CITROEN', '2025-02-26 00:00:00', 'Belgenhoek', 'Grashoek'),
-- ('95-JJZ-8', 4.99638913, 52.03711221, '2024-08-25 01:58:19', 'CITROEN C1', 'CITROEN', '2025-04-12 00:00:00', 'Stuivenbergweg', 'IJsselstein'),
-- ('GJG-83-G', 6.34332818, 51.90590218, '2024-08-27 18:46:29', 'T-ROC', 'VOLKSWAGEN', '2028-08-27 00:00:00', 'Bluemerstraat', 'Etten'),
-- ('98-ZR-NT', 6.01882437, 51.89523855, '2024-08-31 05:31:34', 'CIVIC 4DR HYBRID', 'HONDA', '2025-05-17 00:00:00', 'Rijndijk', 'Doornenburg'),
-- ('95-JK-XP', 5.99248976, 51.38521488, '2024-08-27 15:27:27', 'CORSA-C', 'OPEL', '2025-04-17 00:00:00', 'Schatbroekdijk', 'Sevenum'),
-- ('95-JJ-SH', 5.83254087, 51.43850452, '2024-08-29 18:35:15', 'MEGANE SCENIC', 'RENAULT', '2023-06-02 00:00:00', 'Zonnewende', 'Deurne'),
-- ('95-JPN-3', 5.79491358, 51.36680546, '2024-08-30 16:55:39', 'FIESTA', 'FORD', '2024-12-16 00:00:00', 'Goudplevierweg', 'Heusden'),
-- ('95-JR-FT', 6.1663092, 52.25333786, '2024-08-25 15:10:38', 'Z1 U9', 'BMW', '2013-08-24 00:00:00', 'Boreelplein', 'Deventer');

USE webapi;
INSERT INTO `WebUsers` (`Fullname`, `Username`, `HashedPassword`) 
VALUES 
('John Doe', 'user', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8'); -- password: password
