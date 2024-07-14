import joblib
import pandas as pd
from sklearn.ensemble import RandomForestRegressor
from sklearn.metrics import mean_squared_error, r2_score
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import LabelEncoder

# Load the dataset
df = pd.read_csv('student_performance_data.csv')

# Encode categorical variable 'subject'
label_encoder = LabelEncoder()
df['Subject'] = label_encoder.fit_transform(df['Subject'])

# Define features (X) and targets (y)
X = df[['Subject', 'AverageStudyTime', 'AverageBreakTime']]
y = df[['PredictedGrade', 'PredictedKnowledgeLevel']]  # Predicting both 'PredictedGrade' and 'PredictedKnowledgeLevel'

# Split data into training and testing sets
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

# Initialize the model
model = RandomForestRegressor(random_state=42)

# Train the model
model.fit(X_train, y_train)

# Make predictions on the test set
y_pred = model.predict(X_test)

# Calculate metrics
mse = mean_squared_error(y_test, y_pred)
r2 = r2_score(y_test, y_pred)

print(f"Mean Squared Error: {mse}")
print(f"R^2 Score: {r2}")

# Example of predicting new data
new_data = pd.DataFrame({
    'Subject': label_encoder.transform(['Math']),  # Apply label encoding
    'AverageStudyTime': [70],
    'AverageBreakTime': [15]
})

predicted_values = model.predict(new_data)
print(f"Predicted Grade: {predicted_values[0][0]}")
print(f"Predicted Knowledge Level: {predicted_values[0][1]}")

# Serialize the model and label encoder
model_path = 'trained_model.pkl'
joblib.dump((model, label_encoder), model_path)
