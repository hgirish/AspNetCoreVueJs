import axios from "axios";

export const addProductToCart = ({ state, commit }, product) => {
  const index = findIndex(state.cart, product);

  if (index > 0) {
    commit("updateProductQuantity", index);
  } else {
    commit("addProductToCart", product);
  }
};

export const removeProductFromCart = ({ state, commit }, product) => {
  const index = findIndex(state.cart, product);
  commit("removeProductFromCart", index);
};

export const setProductQuantity = ({ state, commit }, payload) => {
  const index = findIndex(state.cart, payload.product);

  if (payload.quantity > 0) {
    payload.index = index;
    commit("setProductQuantity", payload);
  } else {
    commit("removeProductFromCart", index);
  }
};

export const login = ({ commit }, payload) => {
  return new Promise((resolve, reject) => {
    commit("loginRequest");
    axios
      .post("/api/token", payload)
      .then(response => {
        const auth = response.data;
        axios.defaults.headers.common["Authorization"] = `Bearer ${
          auth.access_token
        }`;
        commit("loginSuccess", auth);
        commit("hideAuthModal");
        resolve(response);
      })
      .catch(error => {
        commit("loginError");
        delete axios.defaults.headers.common["Authorization"];
        reject(error.response);
      });
  });
};

export const register = ({ commit }, payload) => {
  return new Promise((resolve, reject) => {
    commit("registerRequest");
    axios
      .post("/api/account", payload)
      .then(response => {
        commit("registerSuccess");
        resolve(response);
      })
      .catch(error => {
        commit("registerError");
        reject(error.response);
      });
  });
};

export const logout = ({ commit }) => {
  commit("logout");
  delete axios.defaults.headers.common["Authorization"];
};

export const setStripeKey = ({ state, commit }) => {
  if (!(state.stripeKey && state.stripeKey.length > 10))
    axios.get("/api/Orders/StripePublishKey").then(response => {
      var data = response.data;
      commit("setStripeKey", data);
    });
};

function findIndex(cart, product) {
  const index = cart.findIndex(
    i =>
      i.productId === product.productId &&
      i.colourId === product.colourId &&
      i.storageId === product.storageId
  );
  return index;
}
