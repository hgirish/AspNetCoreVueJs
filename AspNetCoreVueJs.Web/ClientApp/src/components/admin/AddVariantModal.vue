<template>
  <div>
    <b-modal ref="modal" id="variantModal" title="Add a new variant" ok-title="Add" @ok="submit">
      <form @submit.prevent="submit">
        <typeahead
          label="Colour"
          name="colour"
          :items="colours"
          v-validate="'required|min:3'"
          :error="vErrors.first('colour')"
          v-model="colour"
        />

        <typeahead
          label="Capacity"
          name="capacity"
          :items="storage"
          v-validate="'required|min:3'"
          :error="vErrors.first('capacity')"
          v-model="capacity"
        />

        <form-input
          type="number"
          label="Price"
          name="price"
          :error="vErrors.first('price')"
          prepend="£"
          v-model="price"
          v-validate="'required|decimal'"
        />
      </form>
    </b-modal>
  </div>
</template>

<script>
import FormInput from "../shared/FormInput.vue";
import Typeahead from "../shared/Typeahead.vue";

export default {
  name: "add-variant-modal",
  components: {
    FormInput,
    Typeahead
  },
  props: {
    colours: {
      type: Array
    },
    storage: {
      type: Array
    }
  },
  data() {
    return {
      colour: "",
      capacity: "",
      price: ""
    };
  },
  methods: {
    submit() {
      this.$validator.validateAll().then(result => {
        if (result) {
          const payload = {
            colour: this.colour,
            storage: this.capacity,
            price: this.price
          };
          this.$emit("submit", payload);
          this.$refs.modal.hide();
          this.colour = "";
          this.capacity = "";
          this.price = "";
        }
      });
    }
  }
};
</script>
