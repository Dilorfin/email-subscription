<template>
	<div class="container-fluid vh-100 d-flex flex-column justify-content-center align-items-center">
		<h1 class="display-1 text-center mb-5">EVEN WIZARDS GO TO WORK</h1>

		<div class="row">
			<div class="d-flex justify-content-center">
				<div class="input-group">
					<input type="email" class="form-control" placeholder="Enter your email" v-model="emailInput"
						:disabled="requestSent">
					<button class="btn btn-outline-dark" @click="onClickSubscribe" :disabled="requestSent">
						<!--https://codepen.io/coopergoeke/pen/wvaYMbJ-->
						<!--https://codepen.io/elifitch/pen/apxxVL-->
						<!--https://codepen.io/aaroniker/pen/bGVGNrV-->
						<span :class="{ 'd-none': !requestSent }" class="spinner-border spinner-border-sm me-2"
							aria-hidden="true"></span>
						<span>Subscribe</span>
					</button>
				</div>
			</div>
			<div>
				<span class="text-warning" :class="{ 'd-none': !showSuccessMessage }">Thank you for subscription!</span>
				<span class="text-danger" :class="{ 'd-none': !showErrorMessage }">{{ errorMessage }}</span>
			</div>
		</div>

		<div class="d-flex justify-content-center mt-5">
			<a href="https://t.me/even_wizards_go_to_work" target="_blank" >
				<img class="logo-link" src="@/assets/icons/telegram.svg" alt="Telegram" width="32">
			</a>
			<a href="https://discord.gg/F3XGNBuKBS" target="_blank" class="ms-3">
				<img class="logo-link" src="@/assets/icons/discord.svg" alt="Discord" width="32">
			</a>
			<a href="https://www.reddit.com/r/EvenWizardsGoToWork/" target="_blank" class="ms-3">
				<img class="logo-link" src="@/assets/icons/reddit.svg" alt="Reddit" width="32">
			</a>
			<a href="https://dilorfin.itch.io/" target="_blank" class="ms-3">
				<img class="logo-link" src="@/assets/icons/itchio.svg" alt="Itch.io" width="32">
			</a>
			<a href="https://twitter.com/dilorfin" target="_blank" class="ms-3">
				<img class="logo-link" src="@/assets/icons/x-twitter.svg" alt="X (Twitter)" width="32">
			</a>
		</div>
		<div class="mt-2">
			<a href="https://www.buymeacoffee.com/dilorfin" target="_blank" class="btn btn-outline-dark">
				<img src="@/assets/icons/bmc-logo.svg" height="24">
				Buy me a coffee
			</a>
		</div>
		</div>
</template>

<script setup lang="ts">

import { ref, type Ref } from 'vue'

const requestSent: Ref<boolean> = ref(false);
const showErrorMessage: Ref<boolean> = ref(false);
const showSuccessMessage: Ref<boolean> = ref(false);

const emailInput: Ref<string> = ref('');
const errorMessage: Ref<string> = ref('');

const validateEmail = (email: string) =>
{
	const matches: RegExpMatchArray | null = email.toLowerCase().match(
		/^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
	);
	return matches && matches.length > 0;
};

async function onClickSubscribe(event: Event)
{
	const errorCodes : Map<number, string> = new Map<number, string>();
	errorCodes.set(0, "Email doesn't seem to be valid email...");
	errorCodes.set(1, "Email is already subscribed");

	showErrorMessage.value = false;
	showSuccessMessage.value = false;

	if (!validateEmail(emailInput.value))
	{
		errorMessage.value = `'${emailInput.value}' doesn't seem to be valid email...`;
		showErrorMessage.value = true;
		return;
	}

	requestSent.value = true;
	const response: Response = await fetch("/api/email/subscribe", {
		method: "POST",
		body: JSON.stringify({ email: emailInput.value })
	});
	requestSent.value = false;

	if (response.ok)
	{
		showSuccessMessage.value = true;
	}
	else 
	{
		const { errorId } : {errorId : number } = await response.json();
		console.log(`bad request, errorId: ${errorId}`);

		errorMessage.value = errorCodes.get(errorId) ?? "Unknown error";
		showErrorMessage.value = true;
	}
}

</script>

<style lang="scss">
.logo-link:hover {
	filter: drop-shadow(0 0 0.035em #000);
}
</style>
