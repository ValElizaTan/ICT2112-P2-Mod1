-- Team 2-3 Seed Data

INSERT INTO Category (Name, Description) VALUES
('Camera', 'Digital and mirrorless cameras'),
('Lens', 'Camera lenses'),
('Tripod', 'Camera tripods and stands'),
('Gimbal', 'Camera stabilizers'),
('Lighting', 'Studio and portable lighting'),
('Microphone', 'Audio recording equipment'),
('Memory Card', 'SD and CFExpress storage cards'),
('Camera Bag', 'Carrying bags and backpacks');

INSERT INTO Product (CategoryId, Sku, Threshold) VALUES
(1, 'CAM-CANON-R5', 0.10),
(1, 'CAM-SONY-A7IV', 0.10),
(2, 'LEN-SONY-2470GM', 0.10),
(2, 'LEN-CANON-70200RF', 0.10),
(3, 'TRI-MANFROTTO-BEFREE', 0.10),
(4, 'GIM-DJI-RS3', 0.10),
(6, 'MIC-RODE-VIDEOMICPRO', 0.10);

INSERT INTO ProductDetails 
(ProductId, TotalQuantity, Name, Description, Weight, Image, Price, DepositRate)
VALUES
(1, 5, 'Canon EOS R5', '45MP full-frame mirrorless camera', 0.74, 'canon_r5.jpg', 150.00, 0.30),
(2, 4, 'Sony A7 IV', '33MP hybrid mirrorless camera', 0.66, 'sony_a7iv.jpg', 130.00, 0.30),
(3, 6, 'Sony FE 24-70mm f2.8 GM', 'Professional standard zoom lens', 0.88, 'sony_2470gm.jpg', 90.00, 0.25),
(4, 3, 'Canon RF 70-200mm f2.8 L', 'Telephoto zoom lens', 1.07, 'canon_70200.jpg', 110.00, 0.25),
(5, 8, 'Manfrotto Befree Advanced Tripod', 'Portable travel tripod', 1.50, 'manfrotto_befree.jpg', 25.00, 0.10),
(6, 4, 'DJI RS 3 Gimbal Stabilizer', '3-axis camera stabilizer', 1.30, 'dji_rs3.jpg', 60.00, 0.20),
(7, 10, 'Rode VideoMic Pro+', 'Shotgun microphone for cameras', 0.12, 'rode_videomic.jpg', 20.00, 0.10);

INSERT INTO InventoryItem (ProductId, SerialNumber) VALUES
(1, 'R5-0001'),
(1, 'R5-0002'),
(1, 'R5-0003'),
(1, 'R5-0004'),
(1, 'R5-0005'),
(2, 'A7IV-0001'),
(2, 'A7IV-0002'),
(2, 'A7IV-0003'),
(2, 'A7IV-0004'),
(3,'2470GM-0001'),
(3,'2470GM-0002'),
(3,'2470GM-0003'),
(3,'2470GM-0004'),
(3,'2470GM-0005'),
(3,'2470GM-0006'),
(4,'70200RF-0001'),
(4,'70200RF-0002'),
(4,'70200RF-0003'),
(5,'TRI-0001'),
(5,'TRI-0002'),
(5,'TRI-0003'),
(5,'TRI-0004'),
(5,'TRI-0005'),
(5,'TRI-0006'),
(5,'TRI-0007'),
(5,'TRI-0008'),
(6,'RS3-0001'),
(6,'RS3-0002'),
(6,'RS3-0003'),
(6,'RS3-0004'),
(7,'RVP-0001'),
(7,'RVP-0002'),
(7,'RVP-0003'),
(7,'RVP-0004'),
(7,'RVP-0005'),
(7,'RVP-0006'),
(7,'RVP-0007'),
(7,'RVP-0008'),
(7,'RVP-0009'),
(7,'RVP-0010');

INSERT INTO "User" (name, email, passwordHash, phoneCountry, phoneNumber)
VALUES
  ('Alice Tan',        'alice.tan@example.com',        '$2b$12$hashAlice', 65, '90000001'),
  ('Benjamin Lee',     'ben.lee@example.com',          '$2b$12$hashBen',   65, '90000002'),
  ('Charlotte Ng',     'charlotte.ng@example.com',     '$2b$12$hashChar',  65, '90000003'),
  ('Daniel Wong',      'daniel.wong@example.com',      '$2b$12$hashDan',   65, '90000004'),
  ('Elaine Goh',       'elaine.goh@example.com',       '$2b$12$hashEla',   65, '90000005'),
  ('Farid Ahmad',      'farid.ahmad@example.com',      '$2b$12$hashFarid', 65, '90000006'),
  ('Grace Lim',        'grace.lim@example.com',        '$2b$12$hashGrace', 65, '90000007'),
  ('Hannah Koh',       'hannah.koh@example.com',       '$2b$12$hashHan',   65, '90000008'),
  ('Ivan Tan',         'ivan.tan@example.com',         '$2b$12$hashIvan',  65, '90000009'),
  ('Jasmine Ong',      'jasmine.ong@example.com',      '$2b$12$hashJas',   65, '90000010'),
  ('Kevin Chan',       'kevin.chan@example.com',       '$2b$12$hashKev',   65, '90000011'),
  ('Lydia Chua',       'lydia.chua@example.com',       '$2b$12$hashLyd',   65, '90000012'),
  ('Marcus Ho',        'marcus.ho@example.com',        '$2b$12$hashMar',   65, '90000013'),
  ('Natalie Yeo',      'natalie.yeo@example.com',      '$2b$12$hashNat',   65, '90000014'),
  ('Operations Admin', 'ops.admin@company.com',        '$2b$12$hashOps',   65, '90000015')
ON CONFLICT (email) DO NOTHING;

INSERT INTO Customer (userId, address, customerType)
VALUES
  (1, '123 Orchard Rd, Singapore 238123', 1),
  (2, '456 Marina Bay, Singapore 018972', 1),
  (3, '10 Tampines Ave, Singapore 529000', 2),
  (4, '5 Jurong East St, Singapore 609000', 2),
  (5, '88 Punggol Walk, Singapore 828000', 1),
  (6, '2 Clementi Rd, Singapore 129000', 2),
  (7, '12 Clementi Ave 1, Singapore 129012', 2),
  (8, '128 Plantation Crescent, Singapore 691128', 1),
  (9, '225 Bukit Batok Central, Singapore 650225', 2),
  (10, '503 Woodlands Ave 14, Singapore 730503', 2),
  (11, '224 Serangoon Ave 4, Singapore 334224', 2)
ON CONFLICT (userId) DO NOTHING;

INSERT INTO Staff (userId, department)
VALUES
  (12, 'Customer Support'),
  (13, 'Operations'),
  (14, 'Finance'),
  (15, 'IT')
ON CONFLICT (userId) DO NOTHING;

-- ============================================================
-- TEAM 1 SEED DATA - INSERT STATEMENTS (In Dependency Order)
-- ============================================================

-- LEVEL 1: Independent Tables (No FK dependencies)
-- ============================================================

-- Insert parent hubs
INSERT INTO transportation_hub (hub_type, longitude, latitude, country_code, address, operational_status, operation_time)
VALUES ('WAREHOUSE', 103.8198, 1.3521, 'SG', '1 Marina Boulevard, Singapore', 'OPERATIONAL', '24/7'),
       ('SHIPPING_PORT', 104.2167, 1.3667, 'SG', 'Port of Singapore, Pasir Ris, Singapore', 'OPERATIONAL', '6AM-11PM'),
       ('AIRPORT', 103.9914, 1.3644, 'SG', 'Changi Airport Terminal 3, Singapore', 'OPERATIONAL', '24/7'),
       ('WAREHOUSE', 114.1694, 22.3193, 'HK', '123 Industrial Road, Hong Kong', 'OPERATIONAL', '8AM-8PM');

-- Insert transport modes
INSERT INTO transport (transport_mode, max_load_kg, vehicle_size_m2, is_available)
VALUES ('TRUCK', 5000.0, 25.0, TRUE),
       ('TRUCK', 8000.0, 35.0, TRUE),
       ('SHIP', 50000.0, 500.0, TRUE),
       ('PLANE', 100000.0, 800.0, TRUE),
       ('TRAIN', 150000.0, 1200.0, TRUE);

-- Insert pricing rules
INSERT INTO pricing_rule (transport_mode, base_rate_per_km, is_active, carbon_surcharge)
VALUES ('TRUCK', 1.5, TRUE, 0.50),
       ('SHIP', 0.5, TRUE, 0.10),
       ('PLANE', 3.0, TRUE, 1.50),
       ('TRAIN', 1.0, TRUE, 0.05);

-- Insert carbon result records
INSERT INTO carbon_result (total_carbon_kg, created_at, validation_passed)
VALUES (250.50, NOW(), TRUE),
       (180.75, NOW(), TRUE),
       (420.30, NOW(), FALSE);

-- Insert product returns
INSERT INTO product_return (return_status, total_carbon, date_in, date_on)
VALUES ('PENDING', 45.25, CURRENT_DATE, CURRENT_DATE),
       ('COMPLETED', 78.90, CURRENT_DATE - INTERVAL '5 days', CURRENT_DATE - INTERVAL '2 days');

-- ============================================================
-- LEVEL 2: Tables with Single Parent Dependencies
-- ============================================================

-- Insert warehouse subtypes
INSERT INTO warehouse (hub_id, warehouse_code, max_product_capacity, total_warehouse_volume, climate_control_emission_rate, lighting_emission_rate, security_system_emission_rate)
VALUES (1, 'WH-SG-001', 10000, 5000.0, 2.5, 1.8, 0.5),
       (4, 'WH-HK-001', 8000, 4000.0, 2.2, 1.6, 0.4);

-- Insert shipping port subtype
INSERT INTO shipping_port (hub_id, port_code, port_name, port_type, vessel_size)
VALUES (2, 'SG-PORT', 'Port of Singapore', 'CONTAINER_PORT', 5000);

-- Insert airport subtype
INSERT INTO airport (hub_id, airport_code, airport_name, terminal, aircraft_size)
VALUES (3, 'SIN', 'Singapore Changi Airport', 3, 400);

-- Insert transport subtypes
INSERT INTO truck (transport_id, truck_id, truck_type, license_plate)
VALUES (1, 1001, 'FLATBED', 'SG-TRUCK-001'),
       (2, 1002, 'BOX_TRUCK', 'SG-TRUCK-002');

INSERT INTO ship (transport_id, ship_id, vessel_type, vessel_number, max_vessel_size)
VALUES (3, 2001, 'CONTAINER_SHIP', 'VESSEL-001', 'Panamax');

INSERT INTO plane (transport_id, plane_id, plane_type, plane_callsign)
VALUES (4, 3001, 'AIRBUS_A330', 'SQ-001');

INSERT INTO train (transport_id, train_id, train_type, train_number)
VALUES (5, 4001, 'FREIGHT', 'TRAIN-001');

-- Insert delivery batches
INSERT INTO delivery_batch (batch_weight_kg, destination_address, delivery_batch_status, total_orders, carbon_savings, hub_id)
VALUES (1500.0, 'Port of Singapore', 'PENDING', 5, 25.50, 1),
       (2000.0, 'Changi Airport', 'SHIPPEDOUT', 8, 35.75, 2),
       (800.0, 'Hong Kong Industrial', 'PENDING', 3, 12.30, 4);
-- ============================================================
-- TEAM 1 SEED DATA - END
-- ============================================================