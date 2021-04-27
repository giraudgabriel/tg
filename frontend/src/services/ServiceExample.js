import baseApi from "~/service";

class ServiceExample {
  async api() {
    const result = await baseApi.post("/");
  }
}

export default new ServiceExample();
