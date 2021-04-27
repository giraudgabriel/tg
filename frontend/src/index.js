import "babel-polyfill";
import React from "react";
import { render } from "react-dom";
import { ConfigProvider } from "antd";
import locale from "antd/es/locale/pt_BR";
import moment from "moment";
import "moment/locale/pt-br";

moment.locale("pt-br"); 

import App from "./App";

render(
  <ConfigProvider locale={locale}>
    <App />
  </ConfigProvider>,
  document.getElementById("app")
);
