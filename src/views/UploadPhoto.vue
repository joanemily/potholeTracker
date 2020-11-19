<template>
        <vue-dropzone
            id="dropzone"
            class="mt-3"
            v-bind:options="dropzoneOptions"
            v-on:vdropzone-sending="addFormData"
            v-on:vdropzone-success="getSuccess"
        ></vue-dropzone>
</template>

 <script>
import vue2Dropzone from "vue2-dropzone";
import "vue2-dropzone/dist/vue2Dropzone.min.css";
export default {
    name: "upload-photo",
    components: {
        vueDropzone: vue2Dropzone
    },
    data(){
        return {
            dropzoneOptions: {
                url: 'https://api.cloudinary.com/v1_1/dlobvad5p/image/upload',  
                thumbnailWidth: 250,
                maxFilesize: 5.0,
                acceptedFiles: ".jpg, .jpeg, .png",
                uploadMultiple: false,
            }, 
        }
    },
 methods:{
          /**
         * Called before sending the request to add additional headers.
         * @function
         */
        addFormData(file, xhr, formData) {
        formData.append("api_key", "zzzzzzz");  // my api key
        formData.append("upload_preset", "zzzzz"); // my upload preset
        formData.append("timestamp", (Date.now() / 1000) | 0);
        formData.append("tags", "pothole-tracker");
        },
        /**
         * Called after an upload success to get the image url.
         * @function
         */
        getSuccess(file, response) {
            const imgUrl = response.secure_url;
            this.$emit("upload", imgUrl);
        },
        
    }
}
</script>
