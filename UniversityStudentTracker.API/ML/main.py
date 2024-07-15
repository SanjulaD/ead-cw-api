import math

import joblib
import numpy as np
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel


# Define a Pydantic model for request body
class PredictionRequest(BaseModel):
    subject: str
    average_study_time: float
    average_break_time: float


app = FastAPI()

# Load the trained model and label encoder
model_path = 'trained_model.pkl'
model, label_encoder = joblib.load(model_path)


@app.post("/predict")
async def predict(data: PredictionRequest):
    try:
        subject_encoded = label_encoder.transform([data.subject])[0]
    except Exception as e:
        raise HTTPException(status_code=400, detail=f"Error encoding subject: {e}")

    # Prepare input data as numpy array
    input_data = np.array([[subject_encoded, data.average_study_time, data.average_break_time]])

    try:
        # Make prediction using the loaded model
        predicted_values = model.predict(input_data)

        # Prepare the response
        response_data = {
            "PredictedGrade": round(float(predicted_values[0][0]), 2),
            "PredictedKnowledgeLevel": math.ceil(predicted_values[0][1]),
        }
        return response_data

    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error predicting grades: {e}")


if __name__ == "__main__":
    import uvicorn

    uvicorn.run(app, host="0.0.0.0", port=8000)
