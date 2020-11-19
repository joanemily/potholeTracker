<template>
  <div id="claim">
    <h2>Make a Claim</h2>
    <form>
      <label>Full Name</label>

      <input type="text" v-model="claim.name" class="item" />

      <label>Phone Number</label>

      <input type="text" v-model="claim.phone" class="item" />

      <label>Address</label>

      <input type="text" v-model="claim.address" class="item" />

      <label>Date of Accident</label>

      <input type="date" v-model="claim.dateOfAccident" class="item" />

      <label>Total Cost of Repairs</label>

      <input type="text" v-model="claim.cost" class="item" />

      <button v-on:click="fileClaim" class="subItem item">Submit Claim</button>

      <router-link :to="{name: 'home'}" tag="button" class="subItem item">Cancel</router-link>

      <a href="../../ClaimForm.docx" download="ClaimForm.docx">
        <button type="button" class="download">Download Claim Form</button>
      </a>
    </form>
  </div>
</template>

<script>
import moment from "moment";
import auth from "@/auth.js";

export default {
  name: "claim",

  data() {
    return {
      showUpload: false,
      claim: {
        id: 0,
        name: "",
        address: "",
        phone: "",
        cost: 0.0,
        dateOfAccident: new Date(),
        potholeId: 0, // TODO allow user to make a claim on a pothole detail page
        username: auth.getUser().sub // Fill in based on user
      },
      filter: null
    };
  },

  methods: {
    formatDate(date) {
      return moment(date, "YYYY-MM-DD HH:mm:ss").format("MMMM DD, YYYY");
    },

    fileClaim() {
      console.log("Claim Filed");
      let url = `${process.env.VUE_APP_REMOTE_API}/claim`;
      let body = JSON.stringify(this.claim);
      console.log(body);

      // fetch here...
      fetch(url, {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
          //Authorization: "Bearer " + auth.getToken()
        },
        body: body
      }).then(response => {
        if (response.ok) {
          alert("Claim has been made!");
          this.$router.push({ name: "home" });
        } else {
          alert(
            `There was an error updating: ${response.status}: ${response.statusText}`
          );
        }
      });
    }
  },
  created() {
    this.claim.potholeId = this.$route.params.id;
  }
};
</script>

<style scoped>
#claim {
  display: flex;
  flex-direction: column;
  justify-content: center;
  min-width: 30%;
  font-family: "Courier New", Courier, monospace;
  background-color: whitesmoke;
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  padding: 10px 0 20px 0;
  position: absolute;
  left: 50%;
  margin-right: -50%;
  transform: translate(-40%);
  padding: 10px 10px 20px 10px;
}

h2 {
  text-align: center;
}

form {
  display: flex;
  flex-direction: column;
  align-items: center;
}

label {
  text-align: center;
  width: 100%;
}

.download {
  padding: 5px;
  margin-top: 10px;
}

@media (max-width: 400px) {
  #claim {
    min-width: 50%;
    margin: none;
  }
  a {
    width: 100%;
  }

}
</style>