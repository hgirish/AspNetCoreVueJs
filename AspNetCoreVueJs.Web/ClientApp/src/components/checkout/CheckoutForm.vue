<template>
  <form @submit.prevent="submit">
    <h4 class="mb-4">Delivery Address</h4>
    <b-row>
      <b-col>
        <b-form-group>
          <label for="firstName">First Name</label>
          <b-form-input
            v-model="firstName"
            data-vv-name="first name"
            v-validate="'required|min:3'"
            :state="state('first name')"
          />
          <b-form-invalid-feedback>
            {{
            vErrors.first("first name")
            }}
          </b-form-invalid-feedback>
        </b-form-group>
      </b-col>
      <b-col>
        <b-form-group>
          <label>Last name</label>
          <b-form-input
            v-model="lastName"
            data-vv-name="last name"
            v-validate="'required|min:3'"
            :state="state('last name')"
          />
          <b-form-invalid-feedback>
            {{
            vErrors.first("last name")
            }}
          </b-form-invalid-feedback>
        </b-form-group>
      </b-col>
    </b-row>

    <b-form-group>
      <label>Address</label>
      <b-form-input
        v-model="address"
        data-vv-name="address"
        v-
        validate="'required'"
        :state="state('address')"
      />
      <b-form-invalid-feedback>
        {{
        vErrors.first("address")
        }}
      </b-form-invalid-feedback>
    </b-form-group>

    <b-form-group>
      <label>
        Address 2
        <span class="text-muted">(Optional)</span>
      </label>
      <b-form-input v-model="address2" data-vv-name="address 2" :state="state('address 2')"/>
    </b-form-group>

    <b-form-group>
      <label>Town / city</label>
      <b-form-input
        v-model="townCity"
        data-vv-name="town / city"
        v-
        validate="'required'"
        :state="state('town / city')"
      />
      <b-form-invalid-feedback>
        {{
        vErrors.first("town / city")
        }}
      </b-form-invalid-feedback>
    </b-form-group>

    <b-form-group>
      <label>County</label>
      <b-form-input
        v-model="county"
        data-vv-name="county"
        v-validate="'required'"
        :state="state('county')"
      />
      <b-form-invalid-feedback>
        {{
        vErrors.first("county")
        }}
      </b-form-invalid-feedback>
    </b-form-group>

    <b-form-group>
      <label>Postcode</label>
      <b-form-input
        v-model="postcode"
        data-vv-name="postcode"
        v-
        validate="'required'"
        :state="state('postcode')"
      />
      <b-form-invalid-feedback>
        {{
        vErrors.first("postcode")
        }}
      </b-form-invalid-feedback>
    </b-form-group>

    <h4 class="mb-4">Payment details</h4>
    <b-form-group>
      <label>Name on card</label>
      <b-form-input
        v-model="nameOnCard"
        data-vv-name="name on card"
        v-validate="'required'"
        :state="state('name on card')"
      />
      <b-form-invalid-feedback>
        {{
        vErrors.first("name on card")
        }}
      </b-form-invalid-feedback>
    </b-form-group>

    <b-form-group>
      <label>Credit/debit card details</label>
      <div ref="card" class="form-control"></div>
    </b-form-group>

    <b-button :disabled="loading" type="submit" variant="primary" size="lg" block class="mt-4 mb-4">
      Checkout
      <span v-if="loading" class="fas fa-spinner fa-spin"></span>
    </b-button>
  </form>
</template>

<script>
import axios from "axios";

export default {
  name: "checkout-form",
  mounted() {
    let stripeKey = this.$store.state.stripeKey;

    /*global  Stripe:true*/
    let stripe = Stripe(`${stripeKey}`);
    let elements = stripe.elements();
    let style = {
      base: {
        lineHeight: "24px"
      }
    };
    let card = elements.create("card", { style: style });
    card.mount(this.$refs.card);
    this.firstName = this.$store.state.auth.firstName;
    this.lastName = this.$store.state.auth.lastName;
    this.card = card;
    this.stripe = stripe;
  },
  beforeDestroy() {
    this.card.destroy();
  },

  data() {
    return {
      nameOnCard: "",
      firstName: "",
      lastName: "",
      address: "",
      address2: "",
      townCity: "",
      county: "",
      postcode: "",
      loading: false,
      stripe: Object,
      card: Object
    };
  },
  methods: {
    state(field) {
      if (this.vErrors.has(field)) {
        return false;
      } else {
        return this.fields[field] && this.fields[field].dirty ? true : null;
      }
    },
    submit() {
      this.$validator.validateAll().then(result => {
        if (result) {
          this.loading = true;
          const details = {
            name: this.nameOnCard
          };

          this.stripe.createToken(this.card, details).then(result => {
            if (result.error) {
              this.loading = false;
            } else {
              const order = {
                stripeToken: result.token.id,
                firstName: this.firstName,
                lastName: this.lastName,
                address1: this.address,
                address2: this.address2,
                townCity: this.townCity,
                county: this.county,
                postcode: this.postcode,
                items: this.$store.state.cart.map(item => {
                  return {
                    productId: item.productId,
                    colourId: item.colourId,
                    storageId: item.storageId,
                    quantity: item.quantity
                  };
                })
              };
              axios
                .post("/api/orders", order)
                .then(response => {
                  this.$store.commit("clearCartItems");
                  this.$emit("success", response.data);
                  this.loading = false;
                })
                .catch(error => {
                  // eslint-disable-next-line no-console
                  console.log(error.response);
                  this.loading = false;
                });
            }
          });
        }
      });
    }
  }
};
</script>

<style lang="scss" scoped>
.StripeElement--focus {
  border-color: #80bdff;
  outline: 0;
  box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
  transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
}
</style>
