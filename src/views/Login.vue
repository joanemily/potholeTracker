<template>
  <div id="login" class="text-center" :style="bg">
    <form class="form-signin" @submit.prevent="login">
      <h1 class="h3 mb-3 font-weight-normal">Please Sign In</h1>
      <div class="alert alert-danger" role="alert" v-if="invalidCredentials">
        Invalid username and password!
      </div>
      <div class="alert alert-success" role="alert" v-if="this.$route.query.registration">
        Thank you for registering, please sign in.
      </div>
      <!--<label for="username" class="sr-only">Username</label>-->
      <input
        type="text"
        id="username"
        class="form-control"
        placeholder="Username"
        v-model="user.username"
        required
        autofocus
      /><br />
      <!--<label for="password" class="sr-only">Password</label>-->
      <input
        type="password"
        id="password"
        class="form-control"
        placeholder="Password"
        v-model="user.password"
        required
      /><br />
      <router-link :to="{ name: 'register' }" class="link">Need an account?</router-link><br />
      <button type="submit" class="gradient-button gradient-button-1">Sign in</button>
    </form>
  </div>
</template>

<script>
import auth from '../auth';

export default {
  name: 'login',
  components: {},
  data() {
    return {
      bg: {backgroundColor: "#e4f2ed"},
      user: {
        username: '',
        password: '',
      },
      invalidCredentials: false,
    };
  },
  methods: {
    login() {
      fetch(`${process.env.VUE_APP_REMOTE_API}/login`, {
        method: 'POST',
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(this.user),
      })
        .then((response) => {
          if (response.ok) {
            return response.text();
          } else {
            this.invalidCredentials = true;
          }
        })
        .then((token) => {
          if (token != undefined) {
            if (token.includes('"')) {
              token = token.replace(/"/g, '');
            }
            auth.saveToken(token);
            this.$router.push('/');
          }
        })
        .catch((err) => console.error(err));
    },
  },
};
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Cairo:wght@700&display=swap');


h1 {
  text-align: center;
  font-family: "Courier New", Courier, monospace;
  font-size: 40px;
  color: orangered;
  margin-bottom: 0px;
}

.form-signin > label, .form-signin > input, .link, button {
  margin: 5px;
}

#login {
  align-items: center;
}

.form-signin {
  text-align: center;
  background-color: whitesmoke;
}

label {
  display: inline-block;
  width: 80px;
}

.form-control {
  padding: 15px;
  border-radius: 20px;
  background-color: #f7f7f7;
  border: solid 1px gray;
}

.form-control:focus {
  outline: none;
}

.gradient-button {
    margin: 10px;
    font-family: "Courier New", Courier, monospace;
    font-size: 10px;
    padding: 10px;
    text-transform: uppercase;
    transition: 0.5s;
    background-size: 200% auto;
    color: #FFF;
    border-radius: 10px;
    width: 100px;
    box-shadow: 0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24);
    transition: all 0.3s cubic-bezier(.25,.8,.25,1);
    cursor: pointer;
    display: inline-block;
    border-radius: 25px;
    border: none;
}
.gradient-button:hover{
    box-shadow: 0 10px 20px rgba(0,0,0,0.19), 0 6px 6px rgba(0,0,0,0.23);
    margin: 8px 10px 12px;
}
.gradient-button:focus {
  outline: none;
}
.gradient-button-1 {background-image: linear-gradient(to right, lightgray 0%, darkgray 51%, Black 100%)}
.gradient-button-1:hover { background-position: right center; }


</style>


