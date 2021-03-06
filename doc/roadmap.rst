Friendly TOM roadmap
====================

There are many features waiting to be implemented in Friendly TOM, as well as
bugfixes. This roadmap sets up milestones for future work.

v0.1: 

- WiX-based installer for proper upgradeability.
- Release under Apache 2.0 license.
    
   - Add license header to all source files
   - add authors file? license header has Friendly TOM team, see AUTHORS
   - AUTHORS has current maintainer and the rest of the team.

- Introduce settings tab and settings for:
  
   - database backup and restore
   - about and credits 

v0.1.1:

- Fix whitespace in Customer/Supplier fields

v0.2.0:

- Fix deletion bug. Currently, if a Supplier is deleted the program will crash
  on next startup because the related payments and bookings have not been 
  deleted.
- Add a Search label to the filtering textbox above DataGrids.
- Add a hint that shows required fields in GUI

v0.2.1:

- Fix Supplier OwnerId. Should be two fields, CI and RUC, and all OwnerIds are
  transferred to RUC.

v0.3:

- Remove all code-level references to Lonely Tree (also in the docs!)

   - Introduces need to rework how Payments work in the code: One abstract
     payment class with ingoing and outgoing subclasses.

- Proper implementation of attachments

   - Fix normalization issue in database
   - Support attachments for all interesting entities (Customers, Suppliers)
   - Support easy backup of attachments

v0.4:

- Database security: SSL/TLS for transit security and database encryption for
  security at rest.
- Proper error messages.

Before introducing new entities / business features, must implement proper
initialization.

Also: documentation!

