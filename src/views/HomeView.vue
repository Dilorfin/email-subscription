<template>
	<div class="container-fluid vh-100 d-flex flex-column justify-content-center align-items-center">
		<h1 class="display-1 text-center mb-5">EVEN WIZARDS GO TO WORK</h1>

		<div class="row">
			<div class="d-flex justify-content-center">
				<div class="input-group mb-3 flex-grow-1">
					<input type="email" class="form-control" placeholder="Enter your email" v-model="emailInput" :disabled="requestSent">
					<button class="btn btn-outline-dark" @click="onClickSubscribe" :disabled="requestSent">
						<!--https://codepen.io/coopergoeke/pen/wvaYMbJ-->
						<!--https://codepen.io/elifitch/pen/apxxVL-->
						<!--https://codepen.io/aaroniker/pen/bGVGNrV-->
						<span :class="{ 'd-none': !requestSent }" class="spinner-border spinner-border-sm me-2" aria-hidden="true"></span>
						<span>Subscribe</span>
					</button>
				</div>
			</div>
		</div>

		<div class="d-flex justify-content-center mt-5">
			<a href="https://dilorfin.itch.io/" target="_blank">
				<img src="@/assets/icons/itchio.svg" alt="Itch.io" width="32">
			</a>
			<a href="https://discord.gg/F3XGNBuKBS" target="_blank" class="ms-3">
				<img src="@/assets/icons/discord.svg" alt="Discord" width="32">
			</a>
			<a href="https://twitter.com/dilorfin" target="_blank" class="ms-3">
				<img src="@/assets/icons/x-twitter.svg" alt="X (Twitter)" width="32">
			</a>
			<a href="https://t.me/untitled_game_by_dilorfin" target="_blank" class="ms-3">
				<img src="@/assets/icons/telegram.svg" alt="Telegram" width="32">
			</a>
			
		</div>
		<div class="mt-2">
			<a href="https://www.buymeacoffee.com/" target="_blank" class="btn btn-outline-dark">
				<img src="@/assets/icons/bmc-logo.svg" height="24">
				Buy me a coffee
			</a>
		</div>
	</div>
</template>

<script setup lang="ts">

import { ref, type Ref } from 'vue'

const requestSent : Ref<boolean> = ref(false);
const emailInput : Ref<string> = ref('');

async function onClickSubscribe(event : Event) {
	requestSent.value = true;
	
	const test = JSON.stringify({ email: emailInput.value });
	const response : Response = await fetch("/api/email/subscribe", {
		method: "POST",
		body: test
	});

	requestSent.value = false;
	if (response.ok)
	{
		console.log("response OK");
	}
	else 
	{
		const { errorId } = await (response).json();
		console.log(`bad request, errorId: ${errorId}`);
	}
}

</script>
