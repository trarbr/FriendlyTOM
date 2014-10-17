-- Sleep time: 0
-- Migrates the customer table

USE [FTOM]
GO

-- Backup and drop Supplier
SELECT * INTO SupplierBackup FROM Supplier
GO
ALTER TABLE PaymentRule
DROP CONSTRAINT FK_PaymentRule_Supplier
GO
ALTER TABLE Booking
DROP CONSTRAINT FK_Booking_Supplier
GO
DROP TABLE Supplier
GO

-- Create new and improved Supplier table
CREATE TABLE [dbo].[Supplier](
    [PartyId] [int] NOT NULL,
    [Type] [nvarchar](50) NOT NULL,
    [AccountNo] [nvarchar](50) NOT NULL,
    [AccountName] [nvarchar](100) NOT NULL,
    [OwnerId] [nvarchar](50) NOT NULL,
    [Bank] [nvarchar](100) NOT NULL,
    [AccountType] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
    [PartyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- Insert data from backup
INSERT INTO Supplier(
    PartyId, 
    Type, 
    AccountNo, 
    AccountName, 
    OwnerId, 
    Bank, 
    AccountType)
SELECT 
    PartyId,
    Type,
    LTRIM(RTRIM(AccountNo)),
    LTRIM(RTRIM(AccountName)),
    LTRIM(RTRIM(OwnerId)),
    LTRIM(RTRIM(Bank)),
    LTRIM(RTRIM(AccountType))
FROM SupplierBackup
GO

-- Add foreign keys back
ALTER TABLE PaymentRule
ADD CONSTRAINT FK_PaymentRule_Supplier
FOREIGN KEY (Supplier)
REFERENCES Supplier(PartyId)
GO
ALTER TABLE Booking
ADD CONSTRAINT FK_Booking_Supplier
FOREIGN KEY (Supplier)
REFERENCES Supplier(PartyId)
GO
ALTER TABLE Supplier
ADD CONSTRAINT FK_Supplier_Party
FOREIGN KEY (PartyId)
REFERENCES Party(PartyId)
GO

-- Drop Supplier backup
DROP TABLE SupplierBackup
GO