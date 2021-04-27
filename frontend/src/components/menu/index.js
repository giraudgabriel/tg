import React from "react";
import styled from "styled-components";

export default function Menu() {
  return (
    <Container>
      <Item>Início</Item>
      <Item>Sobre-nós</Item>
      <Item>Contato</Item>
    </Container>
  );
}

export const Container = styled.nav`
  display: flex;
  flex-flow: row;
  background: #0c0c0c;
  color: #f1f1f1;
  width: 100%;
  position: sticky;
  top: 0;
  height: 35px;
  padding: 16px;
`;

export const Item = styled.nav`
  display: flex;
  flex-flow: row;
  margin: 8px;
  padding: 0 8px;
  cursor: pointer;
  font-size: 21px;
  &:hover {
    filter: opacity(0.8);
  }
`;
