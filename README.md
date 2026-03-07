Elevator Simulation – Unity

Overview
This project simulates a multi-elevator system in Unity where users can request elevators using Up/Down floor buttons.
The system assigns requests to elevators based on distance, movement direction, and current workload to mimic realistic elevator behavior.

Features
Multiple Elevators
• Supports multiple elevators operating independently.
• Each elevator maintains its own request queues.

Floor Request Buttons
• Each floor has Up and Down buttons.
• Pressing a button lights it up until an elevator arrives.

Intelligent Elevator Assignment
When a request is made, the controller selects the most suitable elevator using a scoring system based on:
• Distance to requested floor
• Current number of pending requests
• Movement direction compatibility
This prevents one elevator from handling all requests when others are available.

Direction-based Queues
Each elevator maintains two separate request lists:
• Up requests
• Down requests
Requests are sorted so elevators serve floors in a logical order while moving in the same direction.

Smooth Elevator Movement
Elevators move smoothly between floors and pause briefly when reaching a stop.

Button Reset Logic
When an elevator reaches a floor:
• The corresponding Up or Down button light turns off.
• The request is removed from the elevator queue.

System Behavior
• Elevators can pick up requests while moving if the floor lies in their path.
• Idle elevators are prioritized for new requests when appropriate.
• Multiple elevators can respond to different requests simultaneously.

Known Limitation
If an elevator leaves a floor and the same floor button is pressed immediately, one of the following may occur:
1. The elevator may briefly return to the same floor.
2. The button may light up but the request might not be added to the elevator queue.

This occurs because the system processes requests based on current elevator state and queue synchronization, and rapid button presses can create a temporary mismatch between UI button state and internal request queues.
This behavior does not affect the overall functionality of the system but may appear during rapid repeated input.
