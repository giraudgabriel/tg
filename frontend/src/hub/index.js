import * as signalR from "@microsoft/signalr";

var connection;

const connect = async () => {
  try {
    const options = {
      accessTokenFactory: () => token,
      skipNegotiation: true,
      transport: signalR.HttpTransportType.WebSockets,
    };

    connection = new signalR.HubConnectionBuilder()
      .withUrl(`${apiUrl}hub/iscool`, options)
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.None)
      .build();

    await connection.start();

    connection.onreconnecting((error) => {
      console.error(
        `Conexão perdida com hub iscool: "${error}". Reconectando....`
      );
      dispatch({ type: "@hub/CONNECT", payload: { conn: connection } });
    });

    connection.onreconnected(() => {
      console.log(`Conexão reestabelecida com hub iscool. Conectado!`);
      dispatch({ type: "@hub/CONNECT", payload: { conn: connection } });
    });

    dispatch({ type: "@hub/CONNECT", payload: { conn: connection } });

    return connection;
  } catch (error) {
    console.log(`Erro ao conectar com hub inventory!`);
    console.error(error);
    setTimeout(connect, 1000);
  }
};

export default { connect, connection };
