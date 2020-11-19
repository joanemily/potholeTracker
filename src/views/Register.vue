<template>
  <div id="register" class="text-center" :style="bg">
    <form class="form-register" @submit.prevent="register">
      <h1 class="h3 mb-3 font-weight-normal">Create Account</h1>
      <div class="alert alert-danger" role="alert" v-if="registrationErrors">
        There were problems registering this user.
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
      <input
        type="password"
        id="confirmPassword"
        class="form-control"
        placeholder="Confirm Password"
        v-model="user.confirmPassword"
        required
      /><br />
      <router-link :to="{ name: 'login' }">
        Have an account?
      </router-link><br />
      <button class="gradient-button gradient-button-1" type="submit">
        Create Account
      </button>
    </form>
  </div>
</template>

<script>
export default {
  name: 'register',
  data() {
    return {
      bg: {backgroundColor: "#e4f2ed"},
      user: {
        username: '',
        password: '',
        confirmPassword: '',
        //role: 'user', // dont do this
      },
      registrationErrors: false,
    };
  },
  methods: {
    register() {
      fetch(`${process.env.VUE_APP_REMOTE_API}/register`, {
        method: 'POST',
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(this.user),
      })
        .then((response) => {
          if (response.ok) {
            this.$router.push({ path: '/login', query: { registration: 'success' } });
          } else {
            this.registrationErrors = true;
          }
        })

        .then((err) => console.error(err));
    },
  },
};
</script>

<style>
h1 {
  text-align: center;
  font-family: "Courier New", Courier, monospace;
  font-size: 40px;
  color: orangered;
  margin-bottom: 0px;
}

.form-register {
  text-align: center;
  background: whitesmoke;
}

.form-control {
  padding: 15px;
  margin: 5px;
  border-radius: 20px;
  background-color: #f7f7f7;
  border: solid 1px gray;
}

.form-control:focus {
  outline: none;
  background-color: white;
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
    width: 150px;
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
.gradient-button-1 {background-image: linear-gradient(to right, lightgray 0%, darkgray 51%, Black 100%)}
.gradient-button-1:hover { background-position: right center; }
</style>
