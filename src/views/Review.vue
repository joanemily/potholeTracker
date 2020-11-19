<template>
  <div class="home">
    <h1>Review</h1>
  <div id="pothole" v-for="pothole in potholes" v-bind:key="pothole.id">
        <h3 class="location">{{pothole.Location}}</h3>
    <h4 class="severity">{{pothole.Severity}}</h4>
    <h5>Reported on: {{pothole.dateReported}}</h5>
  </div>
  </div>
</template>

<script>
export default {
  name: 'review',
  data() {
  return {
    potholes: []
  };
},

methods: {
  
   loadPotholes() {
    const url = `${process.env.VUE_APP_REMOTE_API}/review`;
    fetch(url)
    .then(response => {
        if (response.ok) {
            response.json()
            .then(json => {
                this.potholes = json;
            })
        }
        else {
            console.log(`There was an error: ${response}.`)
        }
    }).catch(err => {
        console.log(err);
    })
}
 
},

created() {
  this.loadPotholes();
}
}


</script>

<style scoped>

.home {
  display: flex;
  margin-top: 10px;
}

#pothole {
  border: 1px black solid;
  padding: 5px;
}
</style>