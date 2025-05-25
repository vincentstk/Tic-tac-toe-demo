# Tic-tac-toe-demo
Attached is a demonstration of a Tic-Tac-Toe game implemented with a WebSocket server and a Unity client.

# WebSocket Server

Source Folder: ~/Websocket_server <br/>
Requirements: Node.js, ws package <br/>
To start the server, please run index.js. The server is configured to listen on localhost:8080.<br/>

# Unity Client Game
Source Folder: ~/TicTacToe <br/>
Note: The GameController.cs script manages the connection to the WebSocket server, as well as sending and receiving data. This implementation serves as a simplified demonstration utilizing WebSockets, as I encountered difficulties connecting to the websocket application mentioned in the test. <br/>
Please note that my familiarity with Node.js is limited, and therefore, the security aspects of the WebSocket server may not be robust.<br/>

Thank you for your time and consideration.