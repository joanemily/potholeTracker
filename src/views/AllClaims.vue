<template>
  <div id="content">
      <router-link :to="{name: 'home'}" tag="button">Return</router-link>
    <div id="claims">
      <div v-for="claim in claims" v-bind:key="claim.id" class="claim">
        <h3 id="name">{{claim.name}}</h3>
        <h5 class="phone">Phone: {{claim.phone}}</h5>
        <h5 class="address">Address: {{claim.address}}</h5>
        <h6 class="report">Accident Occurred: {{formatDate(claim.dateOfAccident)}}</h6>
        <h6>Reported on {{formatDate(claim.dateFiled)}}</h6>
        <h4 class="cost">Total Cost: ${{claim.cost}}</h4>
      </div>
    </div>
  </div>
</template>

<script>
import auth from "@/auth.js";
import moment from "moment";

export default {
  name: "home",
  computed: {
    isEmployee() {
      let user = auth.getUser();
      if (user && user.rol === "Employee") {
        return true;
      } else {
        return false;
      }
    }
  },
  data() {
    return {
      claims: []
    };
  },

  methods: {
    loadClaims() {
      const url = `${process.env.VUE_APP_REMOTE_API}/claim/viewAllClaims`;
      console.log(url);
      fetch(url)
        .then(response => {
          if (response.ok) {
            response.json().then(json => {
              this.claims = json;
            });
          } else {
            console.log(`There was an error: ${response}.`);
          }
        })
        .catch(err => {
          console.log(err);
        });
    },

    formatDate(date) {
      return moment(date, "YYYY-MM-DD HH:mm:ss").format("MMMM DD, YYYY");
    }
  },

  created() {
    this.loadClaims();
  }
};
</script>

<style scoped>

#content {
    text-align: center;
}

button {
    padding: 5px 10px;
}


#claims {
  display: flex;
  margin-top: 10px;
  justify-content: center;
  flex-wrap: wrap;
}

.claim {
  background-color: whitesmoke;
  padding: 5px;
  margin: 10px;
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  transition: 0.3s;
  text-align: center;
  font-family: "Courier New", Courier, monospace;
  max-width: 25%;
  min-width: 5%;
  min-height: 4em;
  max-height: 15em;
}

@media (max-width: 800px) {
  .claim {
    background-color: whitesmoke;
    padding: 5px;
    margin: 10px;
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
    transition: 0.3s;
    text-align: center;
    font-family: "Courier New", Courier, monospace;
    max-width: 25%;
    min-width: 5%;
    min-height: 4em;
    max-height: 15em;
  }
  .cost {
    font-size: 12px;
  }
  .phone {
    font-size: 10px;
  }
  .address {
    display: none;
  }
  .report {
    display: none;
  }
}
</style>