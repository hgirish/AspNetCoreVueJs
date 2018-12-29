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

function findIndex(cart, product) {
  const index = cart.findIndex(
    i =>
      i.productId === product.productId &&
      i.colourId === product.colourId &&
      i.storageId === product.storageId
  );
  return index;
}
