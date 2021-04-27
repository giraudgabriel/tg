import React from "react";
import styled from "styled-components";
import Menu from "~/components/menu";

export default function Login() {
  return (
    <Container>
      <Menu />
      <FormContainer>
        <Label>Usuário</Label>
        <Label>Senha</Label>
      </FormContainer>
    </Container>
  );
}
export const Container = styled.div`
  display: flex;
  flex-flow: column;
  width: 100%;
  height: 100%;
`;

export const FormContainer = styled.form`
  display: flex;
  flex-flow: column;
  margin-left: auto;
  margin-right: auto;
  background: #0c0c0c;
  width: 320px;
  height: 320px;
  padding: 64px;
`;

export const Label = styled.label`
  font-size: 16px;
  color: #f1f1f1;
`;
