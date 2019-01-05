import axios from "axios";
import { Validator } from "vee-validate";

const isUniqueName = value => {
  return axios
    .post("/api/products/validate", {
      name: value
    })
    .then(response => {
      // Notice that we return an object containing both a valid property and a data property.
      return {
        valid: response.data.valid,
        data: {
          message: response.data.message
        }
      };
    });
};

Validator.extend("uniqueProductName", {
  validate: isUniqueName,
  getMessage: (field, params, data) => data.message,
  immediate: false
});
