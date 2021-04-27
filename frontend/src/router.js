import React from "react";
import { Router, Switch, Route } from "react-router-dom";
import routes from "./config/routes";
import history from "./history";
import { ConfigProvider } from "antd";

function RouterComponent() {
  return (
    <Router history={history}>
      <ConfigProvider>
        <Switch>
          {routes.map(({ path, component }) => (
            <Route key={path} path={path} component={component} exact />
          ))}
        </Switch>
      </ConfigProvider>
    </Router>
  );
}

export default RouterComponent;
