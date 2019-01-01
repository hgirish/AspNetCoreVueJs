<template>
  <div>
    <h3 class="mt-4">Orders</h3>
    <table
      class="table table-striped table-hover"
      v-if="orders && orders.length > 0"
    >
      <thead>
        <tr>
          <th>Order #</th>
          <th>Placed</th>
          <th>Items</th>
          <th>Total</th>
          <th>Payment Status</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="order in orders" :key="order.id">
          <td>{{ order.id }}</td>
          <td>{{ order.placed | date }}</td>
          <td>{{ order.items }}</td>
          <td>{{ order.total | currency }}</td>
          <td>{{ order.paymentStatus }}</td>
        </tr>
      </tbody>
    </table>
    <div v-else class="alert">You haven't placed any orders yet!</div>
  </div>
</template>

<script>
export default {
  name: "order-list",
  props: {
    orders: {
      type: Array,
      required: false
    }
  }
};
</script>
