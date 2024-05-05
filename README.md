## State builder

I don't know if it seems like a builder pattern, a little bit of a state pattern, or a little chain of responsibility pattern... by the way, for me, it seems like a 'state builder'. It's a builder with a chain of classes with a method that operates over a state. Those classes are enqueued and then dequeued when it is executed.

When I made this solution, I thought about synchronous operations step by step in which operation is tested separately and it is possible to increment operation over and over again... and in the final execution, I can test if a state is consistent. 

Improve testability and cohesion when I have long-run synchronous operations. Separate steps with its business concern, test them individually, and increment as needed.

- initial state  = 1
- add step1 = queue step1
- add step2  = queue step2
- run:
	- operator dequeue step1 
  - step 1 modifies the state to 2 
	- operator dequeue step2
	- step 2 modifies the state to 3... 

and so on... 

Also was made an extension to operate over lists to increment the solution (the reason why this solution was made).

Eg. A list of emails to reset the password, reset 2FA, and add some information from another service.... etc. Add processing state to each item of a list of emails like unprocessed or processed and the final test against the whole processing state or individual email processing state



