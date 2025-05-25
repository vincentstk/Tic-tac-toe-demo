const ws = require('ws');
const wss = new ws.Server({ port: 8080 }, () => {
    console.log('server start on ws://localhost:8080');
});

wss.on('connection', (ws) => {
    ws.on('message', (data) => {
        console.log("string received from client -> '" + data + "'");
        let stringData = data.toString();

        let names = ["Alice", "Bob", "Charlie", "Diana", "Eve"];
        const jsonString = stringData;
        const jsonObject = JSON.parse(jsonString);
        const match = new Match(jsonObject);
        if (match.packageType === 'req/match')
        {
            match.packageType = 'res/match';
            match.botName = names[getRandomInt(names.length)];
            match.botRank = getRandomInt(3);
            const matchData = JSON.stringify(match);
            console.log(matchData);
            ws.send(matchData);
        }
  });
});

wss.on('listening', () => {
    console.log('WebSocket server is listening on port 8080');
});
wss.on('close', function() {
    console.log("client left.");
  });

class Match {
  constructor(data) {
    Object.assign(this, data);
  }
}
function getRandomInt(max) {
  return Math.floor(Math.random() * max);
}