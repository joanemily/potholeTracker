<template>
  <div id="app">
    <div id="img-div">
      <img id="header-image" src="https://www.andymohrcollision.com/wp-content/uploads/2018/11/pot-hole-2-banner.jpg">
    </div>
    <div id="nav">
      <div v-if="user" id="loggedIn">
        <div>
          <button v-on:click="logout" class="navButton">Logout</button>
        </div>
        <div>
          <router-link :to="{name: 'report'}" tag="button" class="navButton">Report a Pothole</router-link>
        </div>
        <div v-if="isEmployee" id="claims">
          <router-link v-if="isEmployee" :to="{name: 'viewClaims'}" tag="button" class="navButton">View All Claims</router-link>
        </div>

      </div>
      <div v-else id="loggedOut">
        <div v-on:click.prevent="showLogin = true">
          <router-link :to="{name: 'login'}" tag="button" class="navButton">Login</router-link>
        </div>
        <div v-on:click.prevent="showRegister = true">
          <router-link :to="{name: 'register'}" tag="button" class="navButton">Register</router-link>
        </div>
      <div v-on:click.prevent="showLogin = false, showRegister = false">
        <router-link to="/" tag="button" class="navButton">Home</router-link>
      </div>
      </div>
    </div>
    <router-view />
  </div>
</template>

<script>
import auth from "@/auth.js";

export default {

  computed: {
    isEmployee() {
      //let user = auth.getUser();
      if (this.user && this.user.rol === "Employee") {
        return true;
      } else {
        return false;
      }
    },
  },
  data() {
    return {
      user: null,
      showLogin: false,
      showRegister: false,
    };
  },
  methods: {
    logout() {
      auth.logout();
      this.$router.push('/login');
      this.user = null;
    },
  },
  created() {
    this.user = auth.getUser();
  },
  watch: {
    $route: function() { // (to, from)
      // react to route changes...
      this.user = auth.getUser();
    }
  }
};
</script>

<style scoped>
#img-div{
  text-align: center;
}

#header-image {
  display: block;
  width: 100%;
  height: auto;
}

#loggedOut > div {
  margin: 20px;
}

#loggedIn > div {
  margin: 20px;
}

#loggedOut {
  display: flex;
  flex-direction: row;
  justify-content: center;
}

#loggedIn {
  display: flex;
  flex-direction: row;
  justify-content: center;
}

button {
  border: none;
  padding: 5px;
  background-color: whitesmoke;
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
  transition: 0.3s;
  text-align: center;
  font-family: "Courier New", Courier, monospace;
  width: 200px;
  border-radius: 25px;
}

@media only screen and (min-width: 1024px){
#loggedIn {
  display: flex;
  flex-direction: row;
  justify-content: center;
  
}

#loggedIn > div {
  margin: 20px;
}

#loggedOut {
  display: flex;
  flex-direction: row;
  justify-content: center;
}

#loggedOut > div {
  margin: 20px;
}

button {
  border: none;
  padding: 5px;
  background-color: whitesmoke;
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
  transition: 0.3s;
  text-align: center;
  font-family: "Courier New", Courier, monospace;
  width: 200px;
  border-radius: 25px;
}

button:hover {
    box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);  
}
}

@media (max-width: 720px) {
  .navButton {
    width: 130px;
  }
}

@media (max-width: 475px) {
  .navButton {
    width: 100px;
  }
}

@media (max-width: 400px) {

  #app {
    width: 100%;
    margin:0%;
    padding: 0%;
  }

  button {
    width: 100%;
    margin: 10px;
    justify-content: center;
  }
  #header-image {
    display: none;
  }
  #nav {
    align-content: center;
  }
  #claims {
    display: none;
  }
}

</style>