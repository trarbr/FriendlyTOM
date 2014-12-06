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

v0.1.2:

- Fix Supplier OwnerId. Should be two fields, CI and RUC, and all OwnerIds are
  transferred to RUC.
- Add a Search label to the filtering textbox above DataGrids.
- Add a hint that shows required fields in GUI

v0.2:

- Remove all code-level references to Lonely Tree (also in the docs!)

   - Introduces need to rework how Payments work in the code: One abstract
     payment class with ingoing and outgoing subclasses.

- Proper implementation of attachments

   - Fix normalization issue in database
   - Support attachments for all interesting entities (Customers, Suppliers)
   - Support easy backup of attachments

v0.3:

- Database security: SSL/TLS for transit security and database encryption for
  security at rest.
- Proper error messages.

Before introducing new entities / business features, must implement proper
initialization.

Also: documentation!

Actions
-------

A thing that I believe will help any tour operators workflow is to generate a
list of to-do actions for each day based on the current "state of business".

E.g: Make a payment, confirm a booking, place a booking, call
hotel/restaurant/etc? to make sure they're ready for guests.

New booking form submitted -> new tentative sale. Enter loop between customer
and sales rep figuring out itinirary, price figured out, then agreement -> 
new planned sale. Sales/Ops people place blockings, sale confirmation deadline 
figured out, then confirmed -> new confirmed sale, ops people confirm blockings, 
payments move from tentative to confirmed (or only confirm on invoice?), people
arrive -> new running sale, make sure all arrangements go as planned, people
leave -> new archived sale, everyone is super happy!
