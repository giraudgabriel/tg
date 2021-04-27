import React from "react";
import Router from "./router";
import { ThemeProvider } from "styled-components";
import { theme } from "~/themes";
import { GlobalStyles } from "~/themes/global";
import "./styles/main.less";

function App() {
  return (
    <ThemeProvider theme={theme}>
      <>
        <GlobalStyles />
        <Router />
      </>
    </ThemeProvider>
  );
}

export default App;
