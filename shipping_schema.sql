-- Create missing tables for P2-4 Shipping Module
CREATE TYPE shipment_priority_enum AS ENUM ('Low', 'Medium', 'High', 'Urgent');

CREATE TABLE IF NOT EXISTS CarrierAgent (
    carrierId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    isActive BOOLEAN NOT NULL DEFAULT true,
    capacity INT NOT NULL,
    servicesAvailable TEXT
);

CREATE TABLE IF NOT EXISTS ShipmentStatusHistory (
    historyId INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    trackingId INT NOT NULL,
    status shipment_status_enum NOT NULL,
    timestamp TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updatedBy VARCHAR(100),
    remark TEXT
);

-- Modify existing Shipment table to match our domain model
ALTER TABLE Shipment ADD COLUMN IF NOT EXISTS dispatchStatus BOOLEAN DEFAULT false;
ALTER TABLE Shipment ADD COLUMN IF NOT EXISTS carrierId INT;
ALTER TABLE Shipment ADD COLUMN IF NOT EXISTS priority shipment_priority_enum DEFAULT 'Medium';
ALTER TABLE Shipment ADD COLUMN IF NOT EXISTS estimatedArrival TIMESTAMPTZ;
ALTER TABLE Shipment RENAME COLUMN destination TO destinationAddress;

-- Insert test data
INSERT INTO CarrierAgent (name, isActive, capacity, servicesAvailable)
VALUES ('FastShip Express', true, 50000, 'GROUND,EXPRESS');

INSERT INTO Shipment (orderId, batchId, status, weight, destinationAddress, dispatchStatus, carrierId, priority, estimatedArrival)
VALUES (1, 1, 'PENDING', 25.5, '123 Main St, San Francisco, CA', true, 1, 'High', NOW() + INTERVAL '3 days');

INSERT INTO ShipmentStatusHistory (trackingId, status, updatedBy, remark)
VALUES (1, 'PENDING', 'system', 'Shipment created');