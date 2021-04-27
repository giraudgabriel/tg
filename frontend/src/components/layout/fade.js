import React, { useEffect, useState } from "react";
import styled from "styled-components";

const Fade = ({ show, children, className, destroy }) => {
  const [shouldRender, setRender] = useState(show);

  useEffect(() => {
    if (show) setRender(true);
  }, [show]);

  const onAnimationEnd = () => {
    if (!show && destroy) setRender(false);
  };

  return (
    shouldRender && (
      <StyledFade
        className={className}
        show={show}
        onAnimationEnd={onAnimationEnd}
      >
        {children}
      </StyledFade>
    )
  );
};

export const StyledFade = styled.div`
  animation: ${({ show }) => (show ? "fadeIn" : "fadeOut")} 0.3s;
  display: ${({ show }) => (show ? "block" : "none")};
  @keyframes fadeIn {
    0% {
      opacity: 0;
    }

    100% {
      opacity: 1;
    }
  }

  @keyframes fadeOut {
    0% {
      opacity: 1;
    }

    100% {
      opacity: 0;
    }
  }
`;

export default Fade;
