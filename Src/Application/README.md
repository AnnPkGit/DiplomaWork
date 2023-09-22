# Remarks

1. Do not call the SaveChanges method in event handlers.


2. When creating an event handler, do not call setters in the implementation of which there is a call to another event, because the latter will not be executed.


3. You can explicitly call the event through a mediator, but note that all state changes of the tracked entities inside the called event will not be saved.