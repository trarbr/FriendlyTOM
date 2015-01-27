Ideas
=====

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

----

infrastructure rebuild: 

- need to ensure only one Model object per Entity. Can be done by restructuring
  the Collections so they know each other.
- cache in DA is OK - useful when reading, to avoid many connections to db
- remove the Models direct dependence on Entities from DA. Should Id and stuff
  like that be available in the Domain? An object reference is just as much an
  Id anyway... you could even put it in the interface! And just keep dirty
  hands away.
- when a supplier is deleted, all payments and payment rules should be deleted
  as well 


