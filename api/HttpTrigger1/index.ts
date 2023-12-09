import { app, HttpRequest, HttpResponseInit, InvocationContext } from "@azure/functions";

interface EmailModel
{
	email: string
};

const validateEmail = (email : string) =>
{
	const matches : RegExpMatchArray = email.toLowerCase()
		.match(
			/^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
		);
		
	return matches?.length > 0;
};

export async function httpTrigger1(request: HttpRequest, context: InvocationContext): Promise<HttpResponseInit>
{
	const model: EmailModel = await request.json() as EmailModel;

	if (!validateEmail(model.email))
	{
		return {
			status: 400,
			jsonBody: {
				error: 0,
				message: `"${model.email}" doesn't seem to be valid email`
			}
		};
	}

	context.log(`input: "${model.email}"`);

	return { status: 200, body: `input: ${model.email}` };
};

app.http('httpTrigger1', {
	methods: ['GET', 'POST'],
	authLevel: 'anonymous',
	handler: httpTrigger1
});
