from PIL import Image
import pyodbc
import os
import random
import string
import io


# Your existing connection string from .NET
connection_string = (
    "Driver={SQL Server};"
    "Server=.;"
    "Database=NaughtyChoppersDB;"
    "Trusted_Connection=yes;"
)

# List of random names
names = 'Olivia Liam Emma Noah Ava Isabella Sophia Mia Jackson Aiden Lucas Ethan Oliver Caden Charlotte Amelia Harper Evelyn Abigail Emily James Benjamin Elijah Daniel William Michael Alexander Sebastian Matthew Ava Sofia Scarlett Grace Lily Zoe Chloe Mia Harper Emily Avery Evelyn Ella Layla Lucy Sophia Zoey Mila Aubrey Riley Ellie Aria Grace Hannah Hailey Amelia Anna Elizabeth Leah Victoria Lillian Eleanor Addison Brooklyn Zoe Nora Lily Scarlett Stella Maya Aurora Natalie Ellie Hazel Violet Penelope Samantha Kennedy Leah Sarah Skylar Audrey Paisley Brooklyn Savannah Bella Claire Skylar Samantha Paisley Bella Claire Aubree Camila Hannah Kinsley Savannah Addison Scarlett Victoria Aria'.split()

# Establish a connection
connection = pyodbc.connect(connection_string)
cursor = connection.cursor()

## Create x amount of users

# Define the number of random records to generate
num_records = int(input('How many records do you want to insert? '))

# Generate and insert random data
for i in range(num_records):
    random_user_username = ''.join(random.choice(string.ascii_letters) for _ in range(10))
    random_user_password = ''.join(random.choice(string.ascii_letters) for _ in range(10))

    # SQL query to insert data
    insert_query = f'INSERT INTO Users (Username, Password) VALUES (?, ?)'

    # Execute the query
    cursor.execute(insert_query, (random_user_username, random_user_password))
    connection.commit()

select_query = (f'SELECT users.Id FROM Users '
                f'LEFT JOIN ProfileInformation ON Users.id = ProfileInformation.UserId '
                f'WHERE ProfileInformation.UserId IS NULL')

cursor.execute(select_query)
user_ids = cursor.fetchall()

## Make sure all users has a profile

# Get a random image from a directory set by the user
directory_path = input('Which folder do you want to take images from? (fullpath)\n')

def encode_image_to_byte_array(image_path):
    image = Image.open(image_path)
    image_byte_array = io.BytesIO()
    image.save(image_byte_array, format=image.format)
    return image_byte_array.getvalue()

# C:\Users\alexa\Documents\helicopters
def get_random_image_byte_array(directory_path):
    # List all image files in the directory
    image_files = [f for f in os.listdir(directory_path) if f.lower().endswith(('.png', '.jpg', '.jpeg', '.gif'))]
    
    if not image_files:
        print("No image files found in the directory.")
        return None
    
    # Choose a random image from the list
    random_image_filename = random.choice(image_files)
    random_image_path = os.path.join(directory_path, random_image_filename)
    image_byte_array = encode_image_to_byte_array(random_image_path)

    
    return image_byte_array

# Get all postal codes
select_query = f'SELECT PostalCode FROM PostalCodes'
cursor.execute(select_query)
postal_codes = cursor.fetchall()

for user in user_ids:
    random_name = random.choice(names)
    random_date = f'{random.randint(1950, 2020)}-{random.randint(1, 12)}-{random.randint(1, 12)}'
    random_model = random.randint(1, 7)
    random_profileimg = get_random_image_byte_array(directory_path)
    random_postalcode = random.choice(postal_codes)[0]
    random_bot = 1
    random_user = user[0]

    # SQL query to insert data
    insert_query = f'INSERT INTO ProfileInformation (Name, Age, Model, ProfileImg, PostalCode, Bot, UserId) VALUES (?, ?, ?, ?, ?, ?, ?)'

    # Execute the query
    cursor.execute(insert_query,
                   (random_name, random_date, random_model, random_profileimg, random_postalcode, random_bot, random_user))
    connection.commit()

select_query = (f'SELECT ProfileInformation.Id FROM ProfileInformation '
                f'LEFT JOIN ProfileModelInterest ON ProfileInformation.Id = ProfileModelInterest.ProfileId '
                f'WHERE ProfileModelInterest.ProfileId IS NULL')
cursor.execute(select_query)
profile_ids = cursor.fetchall()

for profile in profile_ids:
    available_model_interests = list(range(1, 8))  # Model interests range from 1 to 7
    random.shuffle(available_model_interests)

    random_interest_amount = random.randint(1, 3)

    for i in range(random_interest_amount):
        random_model_interest = available_model_interests.pop()

        insert_query = f'INSERT INTO ProfileModelInterest (ProfileId, ModelId) VALUES (?, ?)'

        cursor.execute(insert_query, (profile[0], random_model_interest))
    connection.commit()

select_query = (f'SELECT ProfileInformation.Id FROM ProfileInformation '
                f'LEFT JOIN ProfileInterests ON ProfileInformation.Id = ProfileInterests.ProfileId '
                f'WHERE ProfileInterests.ProfileId IS NULL')
cursor.execute(select_query)
profile_ids = cursor.fetchall()

for profile in profile_ids:
    available_interests = list(range(1, 26))  # Model interests range from 1 to 7
    random.shuffle(available_interests)

    for i in range(3):
        random_interest = available_interests.pop()

        insert_query = f'INSERT INTO ProfileInterests (ProfileId, InterestId) VALUES (?, ?)'

        cursor.execute(insert_query, (profile[0], random_interest))
    connection.commit()

connection.commit()

# Close the connectio
connection.close()

print(f"{num_records} records inserted successfully.")
